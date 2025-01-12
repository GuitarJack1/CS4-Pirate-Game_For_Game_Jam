using UnityEngine;
using System.Collections;

public class Bullet_Script : MonoBehaviour
{
    public float force = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision){
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
