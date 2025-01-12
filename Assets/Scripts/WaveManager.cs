using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float length = 3f;
    [SerializeField] private float speed = .5f;
    [SerializeField] private float offset = 0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public float GetWaveHeight(float x, float z)
    {

        // tried to make it vary
        float wave1 = amplitude * Mathf.Sin((x + z) / length + Time.time * speed + offset);
        float wave2 = (amplitude * 0.5f) * Mathf.Sin((x - z) / (length * 1.5f) + Time.time * speed * 1.2f);
        float wave3 = (amplitude * 0.25f) * Mathf.Sin((x * 0.5f + z) / (length * 0.75f) + Time.time * speed * 0.5f);

        // combine the waves
        float combinedWaveHeight = wave1 + wave2 + wave3;
        return combinedWaveHeight;
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }
}