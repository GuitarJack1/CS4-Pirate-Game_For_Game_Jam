using UnityEngine;
using System.Collections;

public class Bullet_Script : MonoBehaviour
{
    public bool cannonBall;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public float shrapnelForce;
    void OnCollisionEnter(Collision collision)
    {
        if(cannonBall){
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 4.0f);
            for(int i = 0; i < 5; i++){
                GameObject shrapnel = Instantiate(bulletPrefab, new Vector3(transform.position.x + Mathf.Cos(Mathf.PI * (i * 72) / 180), transform.position.y + 1, transform.position.z + Mathf.Sin(Mathf.PI * (i * 72) / 180)), transform.rotation);
                shrapnel.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(Mathf.PI * (i * 72) / 180), 1, Mathf.Sin(Mathf.PI * (i * 72) / 180)) * shrapnelForce, ForceMode.VelocityChange);
            }
        }
        if (!collision.gameObject.CompareTag("Fish"))
        {
            Destroy(gameObject);
        }
    }
}
