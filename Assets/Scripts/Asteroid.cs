using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float radius;
    public float mineralRating;
    public int numberOfVertices;

    private Vector3[] newVertices;
    public Vector2[] newUV;
    private int[] newTriangles;

    private void CreateTriangles()
    {
        List<int> tris = new List<int>();
        for (int i = 1; i < newVertices.Length; ++i)
        {
            tris.Add(0);
            tris.Add(i);
            if (i + 1 < newVertices.Length) tris.Add(i + 1);
        }
        tris.Add(0);
        newTriangles = tris.ToArray();
    }

    private void CreateVertices()
    {
        List<Vector3> vertices = new List<Vector3>();
        float theta = (2f * Mathf.PI);
        while (theta > 0 + (Mathf.Epsilon))
        {
            float randRad = Random.Range(radius * .90f, radius * 1.10f);
            float x = randRad * Mathf.Cos(theta);
            float y = randRad * Mathf.Sin(theta);
            vertices.Add(new Vector3(x, y));
            theta -= (2f * Mathf.PI) / (float)numberOfVertices;
        }
        newVertices = vertices.ToArray();
    }

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
        Vector2[] points = new Vector2[(newVertices.Length / 2) + 1];
        for (int i = 0; i < newVertices.Length / 2; i++)
        {
            points[i] = newVertices[i];
        }
        points[points.Length] = newVertices[0];
        collider.points = points;
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