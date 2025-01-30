using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    Tentacle_Target_Behavior tentTargBehavior;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Projectiles"))
        {
            tentTargBehavior.Hit();
            Destroy(other.gameObject);
        }
    }
}
