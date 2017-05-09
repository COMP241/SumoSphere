using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LevelLoader : MonoBehaviour
{
    private ImageMap map;

    // Generated Fields
    private float horizontalScale;
    private float verticalScale;
    private Vector3 wallHeight;
    private Vector3 adjust;

    // Editor Fields
    [Header("Function")]
    [SerializeField] private Transform playContainer;
    [SerializeField] private float allScale = 10f;

    [Header("Aesthetic")]
    [SerializeField] private Material wallMaterial;
    
    private IEnumerator Start()
    {
        int id = 0;
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

        wallHeight = Vector3.up * allScale / 10f;
        adjust = new Vector3(-horizontalScale * allScale / 2f, 0, verticalScale * allScale / 2f);
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
        foreach (Line line in map.Lines)
        {
            // Set up wall object
            GameObject wall = new GameObject("Wall", typeof(MeshCollider), typeof(MeshRenderer), typeof(MeshFilter));
            Mesh mesh = new Mesh();
            wall.GetComponent<MeshFilter>().mesh = mesh;
            wall.GetComponent<MeshCollider>().sharedMesh = mesh;
            wall.GetComponent<MeshRenderer>().material = wallMaterial;
            wall.transform.parent = playContainer;
            wall.transform.position = Vector3.zero;

            // Set up required collections for mesh
            Vector3[] vertices = new Vector3[line.Points.Length * 2];
            int[] triangles = new int[(vertices.Length - (line.Loop ? 0 : 2)) * 6];
            // TODO: UVs and normals

            Point[] points = line.Points;
            
            // Vertices generation. Grey magic. Probably don't touch.
            for (int p = 0; p < points.Length; p++)
            {
                Vector3 floorVector = new Vector3(points[p].X * horizontalScale * allScale, 0f, -points[p].Y * verticalScale * allScale) + adjust;
                vertices[p * 2] = floorVector;
                vertices[p * 2 + 1] = floorVector + wallHeight;
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

                // Reverse of previous two (so both sides of mesh are visible)
                triangles[i + 6] = triangles[i + 2];
                triangles[i + 7] = triangles[i + 1];
                triangles[i + 8] = triangles[i];

                triangles[i + 9] = triangles[i + 5];
                triangles[i + 10] = triangles[i + 4];
                triangles[i + 11] = triangles[i + 3];
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }
}