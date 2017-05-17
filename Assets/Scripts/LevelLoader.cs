using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class LevelLoader : MonoBehaviour
{
    private ImageMap map;

    // Generated Fields
    private float horizontalScale;
    private float verticalScale;
    private Vector3 adjust;
    private Vector3 spawnPoint;

    // Editor Fields
    [Header("Function")]
    [SerializeField] private Transform playContainer;
    [SerializeField] private float allScale = 10f;
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private GameObject player;

    [Header("Aesthetic")]
    [SerializeField] private Material lineMaterial;
    
    private IEnumerator Start()
    {
        int id = 1;
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
            }
        }
    }

    private void GenerateLevel()
    {
        SetConstants();
        MakeFloor();
        MakeWalls();
        playContainer.gameObject.SetActive(true);
        foreach (GameObject o in objectsToEnable)
            o.SetActive(true);
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

        Line spawnLine = map.Lines.First(l => l.Color == MapColor.Blue);
        player.transform.position = PointToWorldSpace(spawnLine.AveragePoint()) + Vector3.up * 0.5f;
        // TODO: Scaling player / map appropriately
    }

    private void MakeFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.parent = playContainer;
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(horizontalScale * allScale / 10f, 1, verticalScale * allScale / 10f);
    }

    private void MakeWalls()
    {
        foreach (Line line in map.Lines.Where(l => l.Color == MapColor.Black))
        {
            // Set up wall object
            GameObject wall = new GameObject("Wall", typeof (MeshCollider), typeof (MeshFilter), typeof(LineRenderer));

            // Set up GameObject
            Mesh mesh = LineToMeshComponents(line);
            wall.GetComponent<MeshFilter>().mesh = mesh;
            wall.GetComponent<MeshCollider>().sharedMesh = mesh;
            wall.transform.parent = playContainer;
            wall.transform.position = Vector3.zero;

            // Set up line renderer
            LineRenderer renderer = wall.GetComponent<LineRenderer>();
            renderer.positionCount = line.Points.Length;
            renderer.SetPositions(line.Points.Select(p => PointToWorldSpace(p) + Vector3.up * 0.1f).ToArray());
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.loop = line.Loop;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.material = lineMaterial;
            renderer.startColor = Color.black;
            renderer.endColor = Color.black;
        }
    }

    private Vector3 PointToWorldSpace(Point p)
    {
        return new Vector3(p.X * horizontalScale * allScale, 0f, -p.Y * verticalScale * allScale) + adjust;
    }

    private Mesh LineToMeshComponents(Line line)
    {
        Mesh mesh = new Mesh();
        Point[] points = line.Points;

        // Set up required collections for mesh
        Vector3[] vertices = new Vector3[line.Points.Length * 2];
        int[] triangles = new int[(vertices.Length - (line.Loop ? 0 : 2)) * 6];

        // Vertices generation. Grey magic. Probably don't touch.
        for (int p = 0; p < points.Length; p++)
        {
            Vector3 floorVector = PointToWorldSpace(points[p]);
            vertices[p * 2] = floorVector;
            vertices[p * 2 + 1] = floorVector + Vector3.up;
        }

        // Triangles generation. Black magic. Definitely don't touch.
        int len = vertices.Length; // Any modulus will *only* occur for looping lines
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
            triangles[i + 6] = triangles[i + 2];
            triangles[i + 7] = triangles[i + 1];
            triangles[i + 8] = triangles[i];

            triangles[i + 9] = triangles[i + 5];
            triangles[i + 10] = triangles[i + 4];
            triangles[i + 11] = triangles[i + 3];
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}