using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField]
    private float maxVelocity;
    public Vector3 velocity;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = transform.forward * maxVelocity;
        rb.linearVelocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        rb.linearVelocity = velocity;
        transform.forward = rb.linearVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Tried to destroy a fish. Tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player Projectiles"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
