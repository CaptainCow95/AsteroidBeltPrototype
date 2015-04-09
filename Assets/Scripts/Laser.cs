
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour {

    public float lifeTime;
    public float laserRange;
    public float laserWidth;
    public Vector2 target;
    public Vector2 source;
 
	// Use this for initialization
	void Start () {
        Mesh mesh = new Mesh ();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
	    meshFilter.mesh = mesh;
        List<Vector3> vertices = new List<Vector3>();
        
       
        
        vertices.Add(new Vector3(-laserWidth / 2, laserRange));
        vertices.Add(new Vector3(laserWidth / 2, laserRange));
         vertices.Add(new Vector3(-laserWidth / 2, 0));
         vertices.Add(new Vector3(laserWidth / 2, 0));


        Vector3[] newVertices = vertices.ToArray();
	    mesh.vertices = newVertices;

        int[] newTriangles = {0,1,2,1,3,2};
	    //mesh.uv = newUV;
	    mesh.triangles = newTriangles;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
