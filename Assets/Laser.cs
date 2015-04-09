using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script : MonoBehaviour {

    public float lifeTime;
    public float range;
    public float width;

    public void setup(Vector2 target, Vector2 source)
    {
        gameObject.transform.position = source;
        Mesh mesh = new Mesh ();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
	    meshFilter.mesh = mesh;
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(new Vector3(width / 2, 0));
        vertices.Add(new Vector3(-width / 2, range));
        vertices.Add(new Vector3(width / 2, 0));
        vertices.Add(new Vector3(-width / 2, range));



        Vector3[] newVertices = vertices.ToArray();
	    mesh.vertices = newVertices;

        int[] newTriangles = {0,1,2,1,2,3};
	    //mesh.uv = newUV;
	    mesh.triangles = newTriangles;

    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
