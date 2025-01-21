using UnityEngine;
using System.Collections;

public class Bullet_Script : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Fish"))
        {
            Destroy(gameObject);
        }
    }
}
