using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;
    [SerializeField] private float heightMultiplier = 2f;
    [SerializeField] private float flyingSpeed = 0.01f;
    [SerializeField] private float noiseOffset = 0.4f;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;
    private float flying = 0f;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        uvs = new Vector2[vertices.Length];

        flying += flyingSpeed;
        float zOff = flying;
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            float xOff = 0f;
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(xOff, zOff) * heightMultiplier;
                vertices[i] = new Vector3(x, y, z);
                //uvs[i] = new Vector2(((float)(x + (xSize / 2)) / xSize), ((float)(z + (zSize / 2)) / zSize));
                uvs[i] = new Vector2(x, z);

                i++;
                xOff += noiseOffset;
            }
            zOff += noiseOffset;
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
