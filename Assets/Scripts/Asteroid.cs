using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float mineralRating;
    public Vector2[] newUV;
    public int numberOfVertices;
    public float radius;
    private int[] newTriangles;
    private Vector3[] newVertices;

    private void CreateMesh()
    {
        CreateVertices();
        CreateTriangles();
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

        // vertice count +1 for polygon closure and -1 for removing the 0, 0 point
        Vector2[] points = new Vector2[newVertices.Length];
        for (int i = 1; i < newVertices.Length; i++)
        {
            points[i] = newVertices[i];
        }

        points[points.Length] = points[0];
        collider.points = points;
    }

    private void CreateTriangles()
    {
        List<int> tris = new List<int>();
        for (int i = 2; i < newVertices.Length; ++i)
        {
            tris.Add(0);
            tris.Add(i - 1);
            tris.Add(i);
        }

        tris.Add(0);
        tris.Add(newVertices.Length - 1);
        tris.Add(1);
        newTriangles = tris.ToArray();
    }

    private void CreateVertices()
    {
        List<Vector3> vertices = new List<Vector3> { new Vector3(0, 0, 0) };
        float theta = (2f * Mathf.PI);
        while (theta > 0 + (Mathf.Epsilon))
        {
            float randRad = Random.Range(radius * .90f, radius * 1.10f);
            float x = randRad * Mathf.Cos(theta);
            float y = randRad * Mathf.Sin(theta);
            vertices.Add(new Vector3(x, y));
            theta -= (2f * Mathf.PI) / numberOfVertices;
        }

        newVertices = vertices.ToArray();
    }

    // Use this for initialization
    private void Start()
    {
        CreateMesh();
    }

    // Update is called once per frame
    private void Update()
    {
        float scale = mineralRating / 100f;
        gameObject.transform.localScale = new Vector3(scale, scale, 1);
    }
}