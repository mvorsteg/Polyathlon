using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWaves : Waves
{
    private int dimension = 10;
    public MeshFilter meshFilter;
    private Vector3[] verts;
    private void Start() {
        // mesh setup
        mesh = GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        Debug.Log(verts.Length);
        
    }

    public override float GetHeight(Vector3 position)
    {
        // scale factor and position in local space
        Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        Vector3 localPos = Vector3.Scale((position - transform.position), scale);

        // get edge points
        Vector3 p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        Vector3 p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        // clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, dimension);
        p1.z = Mathf.Clamp(p1.z, 0, dimension);
        p2.x = Mathf.Clamp(p2.x, 0, dimension);
        p2.z = Mathf.Clamp(p2.z, 0, dimension);
        p3.x = Mathf.Clamp(p3.x, 0, dimension);
        p3.z = Mathf.Clamp(p3.z, 0, dimension);
        p4.x = Mathf.Clamp(p4.x, 0, dimension);
        p4.z = Mathf.Clamp(p4.z, 0, dimension);

        // get the max distance to one of the edges and take that to compute max dist
        float max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        float dist = (max - Vector3.Distance(p1, localPos))
                   + (max - Vector3.Distance(p2, localPos))   
                   + (max - Vector3.Distance(p3, localPos))
                   + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        // weighted sum
        float height = mesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
                     + mesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
                     + mesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
                     + mesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));
        // return scale
        return height * transform.lossyScale.y / dist;

    }

    private int index(int x, int z)
    {
        return x * (dimension + 1) + z;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            Debug.Log(mesh.name);
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Gizmos.DrawCube(new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z), Vector3.one * 0.1f);
            }
        }
    }
}