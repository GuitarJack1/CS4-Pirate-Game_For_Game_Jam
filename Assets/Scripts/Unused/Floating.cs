using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public int gridSize = 10; // Number of vertices along one axis
    public float waveAmplitude = 1.0f; // Height of the waves
    public float waveFrequency = 1.0f; // Speed of the wave oscillation
    public float waveSpeed = 1.0f; // How fast the waves move

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private float timeOffset;

    public Transform waveSurface; // Assign the wave mesh object here

    void Start()
    {
        if (waveSurface == null)
        {
            Debug.LogError("Wave surface must be assigned.");
            enabled = false;
            return;
        }

        MeshFilter meshFilter = waveSurface.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("Wave surface does not have a valid MeshFilter or Mesh.");
            enabled = false;
            return;
        }

        mesh = meshFilter.sharedMesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        timeOffset = Random.Range(0f, 100f); // Unique wave phase per instance
    }

    void Update()
    {
        UpdateWaveMesh();
        UpdateObjectPosition();
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
        waveSurface.GetComponent<MeshFilter>().mesh = mesh;
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
