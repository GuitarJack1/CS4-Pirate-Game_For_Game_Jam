using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    [SerializeField]
    private Boid[] boids;

    [SerializeField]
    private Transform boidCenter;
    [SerializeField]
    private float containerRadius;
    [SerializeField]
    private float boundaryForce;

    [SerializeField]
    private float radius;
    [SerializeField]
    private float repulsionRadius;
    [SerializeField]
    private float repulsionForce;

    [SerializeField]
    private GameObject boidPrefab;
    [SerializeField]
    private float spawnRadius;
    [SerializeField]
    private int spawnNumber;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        boids = new Boid[spawnNumber];

        for (int i = 0; i < spawnNumber; i++)
        {

            boids[i] = Instantiate(boidPrefab, transform.position + Random.insideUnitSphere * spawnRadius, Random.rotation).GetComponent<Boid>();
        }
    }

    void Update()
    {
        foreach (Boid boid in boids)
        {
            Vector3 averageDistance = Vector3.zero;
            Vector3 averageVelocity = Vector3.zero;
            int found = 0;
            foreach (Boid otherBoid in boids.Where(b => b != boid))
            {
                Vector3 diff = otherBoid.transform.position - boid.transform.position;
                if (diff.magnitude < radius)
                {
                    averageDistance += diff;
                    averageVelocity += otherBoid.velocity;
                    found++;
                }
            }

            if (found > 0)
            {
                averageDistance /= found;
                averageVelocity /= found;
                boid.velocity += Vector3.Lerp(Vector3.zero, averageDistance, averageDistance.magnitude / radius);
                //boid.velocity += Vector3.Lerp(boid.velocity, averageVelocity, Time.deltaTime);
                boid.velocity -= Vector3.Lerp(Vector3.zero, averageDistance, averageDistance.magnitude / repulsionRadius) * repulsionForce;
            }

            Vector3 vectToContainerCenter = boid.transform.position - boidCenter.position;
            if (vectToContainerCenter.magnitude > containerRadius)
            {
                boid.velocity += vectToContainerCenter.normalized * (containerRadius - vectToContainerCenter.magnitude) * boundaryForce * Time.deltaTime;
            }
        }
    }
}
