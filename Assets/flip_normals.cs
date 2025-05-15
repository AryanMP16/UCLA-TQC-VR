using UnityEngine;
using System.Collections;

public class Flip_Normals : MonoBehaviour 
{
    public GameObject background_cylinder;

    void Start()
    {
        Vector3[] normals = background_cylinder.GetComponent<MeshFilter>().mesh.normals;
        for(int i = 0; i < normals.Length; i++) {
            normals[i] = -normals[i];
        }
        background_cylinder.GetComponent<MeshFilter>().sharedMesh.normals = normals;

        int[] triangles = background_cylinder.GetComponent<MeshFilter>().sharedMesh.triangles;
        for (int i = 0; i < triangles.Length; i+=3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }           

        background_cylinder.GetComponent<MeshFilter>().sharedMesh.triangles= triangles;
    }
}