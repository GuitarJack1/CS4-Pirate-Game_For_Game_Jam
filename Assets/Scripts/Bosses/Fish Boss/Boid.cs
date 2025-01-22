using UnityEngine;
using UnityEngine.UI;

public class Boid : MonoBehaviour
{
    [SerializeField]
    private bool isGoldenFish;
    [SerializeField]
    private float maxVelocity;
    public Vector3 velocity;
    private Rigidbody rb;

    [SerializeField]
    private Color beginningColor;
    [SerializeField]
    private Color deadColor;
    [SerializeField]
    private float timeToDestroyAfterKilled;

    private bool alive;
    private float deathTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = transform.forward * maxVelocity;
        rb.linearVelocity = velocity;
        alive = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<RawImage>().color = beginningColor;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (velocity.magnitude > maxVelocity)
            {
                velocity = velocity.normalized * maxVelocity;
            }

            rb.linearVelocity = velocity;
            transform.forward = rb.linearVelocity;
        }
        else
        {
            if (Time.time > deathTime + timeToDestroyAfterKilled)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player Projectiles"))
        {
            if (!isGoldenFish)
            {
                // Destroy(gameObject);
                alive = false;
                GetComponent<CapsuleCollider>().enabled = false;
                rb.useGravity = true;

                velocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;
                transform.right = Camera.main.transform.position - transform.position;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90f, transform.eulerAngles.y, transform.eulerAngles.z);
                deathTime = Time.time;
            }

            GetComponentInChildren<RawImage>().color = deadColor;
            Destroy(collision.gameObject);
        }
    }
}
