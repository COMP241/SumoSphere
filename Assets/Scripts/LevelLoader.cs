using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CSG;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader instance;

    private ImageMap map;
    private bool loading;

    // Generated Fields
    private float horizontalScale;
    private float verticalScale;
    private Vector3 adjust;
    private GameObject floor;

    // Editor Fields
    [Header("Function")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject playContainer;
    [SerializeField] private float allScale = 10f;

    [Header("Prefabs")]
    [SerializeField] private GameObject goalPrefab;

    [Header("Aesthetic")]
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;

    private void Start()
    {
        if (instance == null)
            instance = GetComponent<LevelLoader>();
        else
            Destroy(gameObject);
    }

    public void Load(string id)
    {
        if (loading)
            return;

        try
        {
            StartCoroutine(LoadLevel(int.Parse(id)));
        }
        catch (FormatException)
        {
            GameStartCanvas.DisplayError("Level ID in wrong format.");
        }
    }
    
    private IEnumerator LoadLevel(int id)
    {
        loading = true;
        GameStartCanvas.DisplayInfo("Loading...");
        using (UnityWebRequest www = UnityWebRequest.Get("http://papermap.tk/api/map/" + id))
        {
            yield return www.Send();

            if (!www.isError)
            {
                map = ImageMap.FromJson(www.downloadHandler.text);
                GenerateLevel();
            }
            else
            {
                Debug.Log(www.error);
                GameStartCanvas.DisplayError("Failed to get map.");
            }
        }
        loading = false;
    }

    private void GenerateLevel()
    {
        try
        {
            SetConstants();
            MakeSpawn();
            MakeFloor();
            MakeWalls();
            MakeObstacles();
            MakeGoals();
            playContainer.SetActive(true);
            GameController.Begin();
        }
        catch (LevelStructureException e)
        {
            GameStartCanvas.DisplayError(e.Message);
        }
        catch (NullReferenceException)
        {
            GameStartCanvas.DisplayError("Failed to get map.");
        }
    }

    private void SetConstants()
    {
        if (map.Ratio >= 1f)
        {
            horizontalScale = map.Ratio;
            verticalScale = 1f;
        }
        else
        {
            horizontalScale = 1f;
            verticalScale = 1f / map.Ratio;
        }
        
        adjust = new Vector3(-horizontalScale * allScale / 2f, 0, verticalScale * allScale / 2f);
    }

    private void MakeSpawn()
    {
        Line spawnLine;

        try
        {
            spawnLine = map.Lines.First(l => l.Color == MapColor.Blue);
        }
        catch (InvalidOperationException)
        {
            throw new LevelStructureException("No spawn in level."); ;
        }

        Point averagePoint = spawnLine.AveragePoint();
        GameController.SetSpawn(PointToWorldSpace(averagePoint) + Vector3.up * 0.5f);
        Player p = playContainer.GetComponentInChildren<Player>();
        p.transform.localScale = Vector3.one * 2f * PointScaleToWorldScale(Mathf.Sqrt(spawnLine.AverageSqrDistanceFrom(averagePoint)));
    }

    private void MakeFloor()
    {
        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.parent = levelContainer;
        floor.transform.position = Vector3.down * 0.25f;
        floor.transform.localScale = new Vector3(horizontalScale * allScale, 0.5f, verticalScale * allScale);
    }

    private void MakeWalls()
    {
        foreach (Line line in map.Lines.Where(l => l.Color == MapColor.Black))
        {
            GameObject wall = new GameObject("Wall", typeof (MeshCollider), typeof(MeshFilter), /*typeof(LineRenderer),*/ typeof(MeshRenderer));

            // Set up GameObject
            Mesh mesh = LineToMeshComponents(line, Vector3.zero);
            wall.GetComponent<MeshCollider>().sharedMesh = mesh;
            wall.GetComponent<MeshRenderer>().sharedMaterial = wallMaterial;
            wall.GetComponent<MeshFilter>().sharedMesh = mesh;
            wall.transform.parent = levelContainer;
            wall.transform.position = Vector3.zero;
//            SetUpLineRenderer(wall, line, Color.black);
        }
    }

    private void MakeObstacles()
    {
        foreach (Line line in map.Lines.Where(l => l.Color == MapColor.Red))
        {
            Mesh mesh = Triangulator.PolygonExtrude(line.Points.Select(PointToWorldSpace).Select(w => new Vector2(w.x, w.z)).ToArray(), Vector3.down, 2f);

            // Set up GameObject
            GameObject obstacle = new GameObject();
            obstacle.AddComponent<MeshFilter>().sharedMesh = mesh;

            Mesh newFloor = CSG.Subtract(floor, obstacle);
            GameObject composite = new GameObject("Floor");
            composite.AddComponent<MeshFilter>().sharedMesh = newFloor;

            Destroy(obstacle);
            Destroy(floor);
            floor = composite;
        }

        // Finalise floor
        floor.AddComponent<MeshCollider>().sharedMesh = floor.GetComponent<MeshFilter>().mesh;
        if (floor.GetComponent<MeshRenderer>() == null)
            floor.AddComponent<MeshRenderer>().material = floorMaterial;
        else
            floor.GetComponent<MeshRenderer>().material = floorMaterial;
        floor.transform.parent = levelContainer;
    }

    private void MakeGoals()
    {
        IEnumerable<Line> lines = map.Lines.Where(l => l.Color == MapColor.Green);

        if (lines.Count() == 0)
            throw new LevelStructureException("No goals in level.");

        foreach (Line line in lines)
        {
            Point averagePoint = line.AveragePoint();
            GameObject goal = Instantiate(goalPrefab, PointToWorldSpace(averagePoint), Quaternion.identity, levelContainer);
            float scale = 2f * PointScaleToWorldScale(Mathf.Sqrt(line.AverageSqrDistanceFrom(averagePoint)));
            goal.transform.localScale = new Vector3(scale, 1, scale);
        }
    }

    private void SetUpLineRenderer(GameObject o, Line line, Color color)
    {
        LineRenderer renderer = o.GetComponent<LineRenderer>();
        renderer.positionCount = line.Points.Length;
        renderer.SetPositions(line.Points.Select(p => PointToWorldSpace(p) + Vector3.up * 0.5f).ToArray());
        renderer.startWidth = 0.1f;
        renderer.endWidth = 0.1f;
        renderer.loop = line.Loop;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.material = lineMaterial;
        renderer.startColor = color;
        renderer.endColor = color;
    }

    private Vector3 PointToWorldSpace(Point p)
    {
        return new Vector3(p.X * horizontalScale * allScale, 0f, -p.Y * verticalScale * allScale) + adjust;
    }

    private float PointScaleToWorldScale(float f)
    {
        return f * allScale * Mathf.Max(verticalScale, horizontalScale);
    }

    private Mesh LineToMeshComponents(Line line, Vector3 offset, float height = 1f)
    {
        Mesh mesh = new Mesh();
        Point[] points = line.Points;

        // Set up required collections for mesh
        Vector3[] vertices = new Vector3[(line.Points.Length + (line.Loop ? 1 : 0)) * 4];
        int[] triangles = new int[(vertices.Length - 4) * 3];

        // Vertices generation. Grey magic. Probably don't touch.
        int p = 0;
        for (; p < points.Length; p++)
        {
            Vector3 floorVector = PointToWorldSpace(points[p]) + offset;
            vertices[p * 2] = floorVector;
            vertices[p * 2 + 1] = floorVector + Vector3.up * height;

            vertices[p * 2 + vertices.Length / 2] = floorVector;
            vertices[p * 2 + 1 + vertices.Length / 2] = floorVector + Vector3.up * height;
        }
        if (line.Loop)
        {
            Vector3 floorVector = PointToWorldSpace(points[0]) + offset;
            vertices[p * 2] = floorVector;
            vertices[p * 2 + 1] = floorVector + Vector3.up * height;

            vertices[p * 2 + vertices.Length / 2] = floorVector;
            vertices[p * 2 + 1 + vertices.Length / 2] = floorVector + Vector3.up * height;
        }

        // Triangles generation. Black magic. Definitely don't touch.
        int len = vertices.Length;
        for (int i = 0; i < triangles.Length; i += 12)
        {
            int start = i / 6;
            // "Lower left" triangle [0,1,2]
            triangles[i] = start;
            triangles[i + 1] = start + 1;
            triangles[i + 2] = (start + 2) % len;

            // "Upper right" triangle [2,1,3]
            triangles[i + 3] = (start + 2) % len;
            triangles[i + 4] = start + 1;
            triangles[i + 5] = (start + 3) % len;

            // Reverse of previous two (so both sides of mesh are working)
            triangles[i + 6] = triangles[i + 2] + len / 2;
            triangles[i + 7] = triangles[i + 1] + len / 2;
            triangles[i + 8] = triangles[i] + len / 2;

            triangles[i + 9] = triangles[i + 5] + len / 2;
            triangles[i + 10] = triangles[i + 4] + len / 2;
            triangles[i + 11] = triangles[i + 3] + len / 2;
        }

        // UV generation
        Vector2[] uvs = new Vector2[vertices.Length];
        float horizontal = 0f;
        int halflen = uvs.Length / 2;
        for (int i = 0; i < halflen; i+=2)
        {
            uvs[i] = uvs[i + halflen] = new Vector2(horizontal, 1f);
            uvs[i + 1] = uvs[i + 1 + halflen] = new Vector2(horizontal, 0f);
            horizontal += Vector3.Distance(vertices[i], vertices[i + 2]);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static void Unload()
    {
        Destroy(instance.levelContainer.gameObject);
        GameObject newContainer = new GameObject("Level Container");
        newContainer.transform.parent = instance.playContainer.transform;
        instance.levelContainer = newContainer.transform;
    }

    public static void SetActive(bool value)
    {
        instance.playContainer.SetActive(value);
    }
}

public class LevelStructureException : Exception
{
    public LevelStructureException() : base()
    {
    }

    public LevelStructureException(string message) : base(message)
    {
    }
}