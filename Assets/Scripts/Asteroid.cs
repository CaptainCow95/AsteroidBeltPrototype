using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsteroidBelt
{
    public class Asteroid : MonoBehaviour
    {
        public Vector2[] newUV;
        public int numberOfVertices;
        public GameObject particleSystemPrefab;
        public float radiusPerMineral;
        private float mineralRating;
        private int[] newTriangles;
        private Vector3[] newVertices;

        public float MineralRating
        {
            get
            {
                return mineralRating;
            }

            set
            {
                mineralRating = value;
                if (mineralRating <= 0)
                {
                    Destroy(gameObject);
                }
                float scale = mineralRating * radiusPerMineral;
                gameObject.transform.localScale = new Vector3(scale, scale, 1);
            }
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

            // vertice count +1 for polygon closure and -1 for removing the 0, 0 point
            Vector2[] points = new Vector2[newVertices.Length];
            for (int i = 1; i < newVertices.Length; i++)
            {
                points[i - 1] = newVertices[i];
            }

            points[points.Length - 1] = points[0];
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
                float randRad = Random.Range(.90f, 1.10f);
                float x = randRad * Mathf.Cos(theta);
                float y = randRad * Mathf.Sin(theta);
                vertices.Add(new Vector3(x, y));
                theta -= (2f * Mathf.PI) / numberOfVertices;
            }

            newVertices = vertices.ToArray();
            newUV = newVertices.Select(e => new Vector2(e.x, e.y)).ToArray();
        }

        // Use this for initialization
        private void Start()
        {
            CreateMesh();
        }
    }
}