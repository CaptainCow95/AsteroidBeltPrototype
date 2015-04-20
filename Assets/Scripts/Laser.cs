using System.Collections.Generic;
using UnityEngine;

namespace AsteroidBelt
{
	public class Laser : MonoBehaviour
	{
		public float laserRange;
		public float laserWidth;
		public float lifeTime;
		public Vector2 source;
		public Vector2 target;

		private MeshRenderer renderer;

		// Use this for initialization
		private void Start()
		{
			renderer = GetComponent<MeshRenderer>();

			Vector2[] newUV = new Vector2[4];
			Mesh mesh = new Mesh();
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			meshFilter.mesh = mesh;

			List<Vector3> vertices = new List<Vector3>();

			vertices.Add(new Vector3(-laserWidth / 2, laserRange));
			newUV[0] = new Vector2(0, 1);
			vertices.Add(new Vector3(laserWidth / 2, laserRange));
			newUV[1] = new Vector2(1, 1);
			vertices.Add(new Vector3(-laserWidth / 2, 0));
			newUV[2] = new Vector2(0, 0);
			vertices.Add(new Vector3(laserWidth / 2, 0));
			newUV[3] = new Vector2(1, 0);

			Vector3[] newVertices = vertices.ToArray();
			mesh.vertices = newVertices;

			int[] newTriangles = { 0, 1, 2, 1, 3, 2 };

			//mesh.uv = newUV;
			mesh.triangles = newTriangles;
			mesh.uv = newUV;
		}

		// Update is called once per frame
		private void Update()
		{
			var color = renderer.material.color;
			color.a -= (1f / lifeTime) * Time.deltaTime;
			if (color.a <= 0)
			{
				Destroy(gameObject);
			}

			renderer.material.color = color;
		}
	}
}