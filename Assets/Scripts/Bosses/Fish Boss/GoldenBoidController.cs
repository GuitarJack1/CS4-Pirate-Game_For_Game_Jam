using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldenBoidController : MonoBehaviour
{
    private Boid boid;

    [SerializeField]
    private Transform boidCenter;
    [SerializeField]
    private float pushToCenterForce;
    [SerializeField]
    private float randomPushForce;
    [SerializeField]
    private GameObject goldenBoidPrefab;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        boid = Instantiate(goldenBoidPrefab, transform.position, Random.rotation).GetComponent<Boid>();
    }

    void Update()
    {
        Vector3 distFromCenter = boidCenter.position - boid.transform.position;
        boid.velocity += Vector3.Lerp(Vector3.zero, distFromCenter, distFromCenter.magnitude) * pushToCenterForce;
        boid.velocity += Random.insideUnitSphere * randomPushForce;
    }
}
