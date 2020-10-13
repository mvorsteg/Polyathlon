using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{

    [System.Serializable]
    public struct Ocatave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    public int dimension = 10;

    public float uvScale;

    public MeshFilter meshFilter;
    public Mesh mesh;
    public Ocatave[] ocataves;

    private void Start() {
        // mesh setup
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTris();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    private void Update()
    {
        Vector3[] verts = mesh.vertices;

        for (int x = 0; x < dimension + 1; x++)
        {
            for (int z = 0; z < dimension + 1; z++)
            {
                float y = 0f;
                for (int o = 0; o < ocataves.Length; o++)
                {
                    if (ocataves[o].alternate)
                    {
                        float perl = Mathf.PerlinNoise((x * ocataves[o].scale.x) / dimension, (z * ocataves[o].scale.y) / dimension) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + ocataves[o].speed.magnitude * Time.time) * ocataves[o].height;
                    }
                    else
                    {
                        float perl = Mathf.PerlinNoise((x * ocataves[o].scale.x + Time.time * ocataves[o].speed.x) / dimension, (z * ocataves[o].scale.y + Time.time * ocataves[0].speed.y) / dimension) * Mathf.PI * 2f;
                        y += perl * ocataves[0].height;
                    }
                }
                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = verts;
        mesh.RecalculateNormals();
    }

    public float GetHeight(Vector3 position)
    {
        // scale factor and position in local space
        Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1/ transform.lossyScale.z);
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
        float height = mesh.vertices[index((int)p1.x, (int)p1.z)].y * Vector3.Distance(p1, localPos)
                     + mesh.vertices[index((int)p2.x, (int)p2.z)].y * Vector3.Distance(p2, localPos)
                     + mesh.vertices[index((int)p3.x, (int)p3.z)].y * Vector3.Distance(p3, localPos)
                     + mesh.vertices[index((int)p4.x, (int)p4.z)].y * Vector3.Distance(p4, localPos);
        // return scale
        return height * transform.lossyScale.y / dist;

    }

    private Vector3[] GenerateVerts()
    {
        Vector3[] verts = new Vector3[(dimension + 1) * (dimension + 1)];

        for (int x = 0; x < dimension + 1; x++)
        {
            for (int z = 0; z < dimension + 1; z++)
            {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    private int[] GenerateTris()
    {
        int[] tris = new int[mesh.vertices.Length * 6];

        for (int x = 0; x < dimension; x++)
        {
            for (int z = 0; z < dimension; z++)
            {
                tris[index(x, z) * 6 + 0] = index(x + 0, z + 0);
                tris[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tris[index(x, z) * 6 + 2] = index(x + 1, z + 0);
                tris[index(x, z) * 6 + 3] = index(x + 0, z + 0);
                tris[index(x, z) * 6 + 4] = index(x + 0, z + 1);
                tris[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }

        return tris;
    }

    private Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        for (int x = 0; x < dimension + 1; x++)
        {
            for (int z = 0; z < dimension + 1; z++)
            {
                Vector2 vec = new Vector2((x / uvScale) % 2, (z / uvScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private int index(int x, int z)
    {
        return x * (dimension + 1) + z;
    }
}