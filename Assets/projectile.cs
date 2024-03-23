using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    // Start is called before the first frame update
    protected MeshFilter meshFilter;
    protected Mesh mesh;
    [SerializeField] float radius = 3f;
    [SerializeField] float density = 1f;
    [SerializeField] Vector3 center = new Vector3(0, 0, 0);
    float circumference;
    int totalVertices;
    int usedVertices;
    [SerializeField] float arcLengthInDegrees = 180f;
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "GeneratedMesh";
        usedVertices = totalVertices;

        circumference = 2 * Mathf.PI * radius;
        totalVertices = Mathf.RoundToInt(circumference * density);

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTries();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        arcLengthInDegrees -= Input.GetAxis("Horizontal");

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTries();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private Vector3[] GenerateVerts()
    {
        usedVertices = (int)(totalVertices * arcLengthInDegrees / 180f);
        Vector3[] vertices = new Vector3[usedVertices + 1];

        vertices[0] = new Vector3(0, 0, 0);

        for (int i = 0; i < usedVertices; i++)
        {
            float angle = 2 * Mathf.PI * i / totalVertices;
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);
            vertices[i+1] = new Vector3(x, 0, z);
        }

        return vertices;
    }

    private int[] GenerateTries()
    {
        int[] verticesToTriangles = new int[usedVertices * 3+3];
        int j = 0;
        for (int i = 0; i < usedVertices - 1; i++)
        {
            verticesToTriangles[i * 3 + 0] = 0;
            verticesToTriangles[i * 3 + 1] = 2+i;
            verticesToTriangles[i * 3 + 2] = 1+i;
            j += 3;
        }
        if (usedVertices == totalVertices)
        {
            verticesToTriangles[j + 0] = 0;
            verticesToTriangles[j + 1] = 1;
            verticesToTriangles[j + 2] = totalVertices;
        }
        return verticesToTriangles;
    }
}