using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FloatingWaveMesh : MonoBehaviour
{
    public int gridSize = 10; // Number of vertices along one axis
    public float waveAmplitude = 1.0f; // Height of the waves
    public float waveFrequency = 1.0f; // Speed of the wave oscillation
    public float waveSpeed = 1.0f; // How fast the waves move

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private float timeOffset;

    void Start()
    {
        GenerateMesh();
        timeOffset = Random.Range(0f, 100f); // Unique wave phase per instance
    }

    void Update()
    {
        UpdateWaveMesh();
        UpdateObjectPosition();
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        originalVertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        displacedVertices = new Vector3[originalVertices.Length];

        Vector2[] uv = new Vector2[originalVertices.Length];
        int[] triangles = new int[gridSize * gridSize * 6];

        float stepSize = 1f / gridSize;
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int y = 0; y <= gridSize; y++)
        {
            for (int x = 0; x <= gridSize; x++)
            {
                originalVertices[vertexIndex] = new Vector3(x * stepSize, 0, y * stepSize);
                uv[vertexIndex] = new Vector2(x * stepSize, y * stepSize);

                if (x < gridSize && y < gridSize)
                {
                    triangles[triangleIndex] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + gridSize + 1;
                    triangles[triangleIndex + 2] = vertexIndex + 1;

                    triangles[triangleIndex + 3] = vertexIndex + 1;
                    triangles[triangleIndex + 4] = vertexIndex + gridSize + 1;
                    triangles[triangleIndex + 5] = vertexIndex + gridSize + 2;

                    triangleIndex += 6;
                }

                vertexIndex++;
            }
        }

        mesh.vertices = originalVertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void UpdateWaveMesh()
    {
        float time = Time.time + timeOffset;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            vertex.y = Mathf.Sin((vertex.x + time * waveSpeed) * waveFrequency) * waveAmplitude
                     + Mathf.Sin((vertex.z + time * waveSpeed) * waveFrequency) * waveAmplitude;
            displacedVertices[i] = vertex;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }

    private void UpdateObjectPosition()
    {
        Vector3 objPosition = transform.position;
        float waveHeight = GetWaveHeightAtPosition(objPosition);
        transform.position = new Vector3(objPosition.x, waveHeight, objPosition.z);
    }

    private float GetWaveHeightAtPosition(Vector3 position)
    {
        float time = Time.time + timeOffset;
        float waveHeight = Mathf.Sin((position.x + time * waveSpeed) * waveFrequency) * waveAmplitude
                         + Mathf.Sin((position.z + time * waveSpeed) * waveFrequency) * waveAmplitude;
        return waveHeight;
    }
}