using UnityEngine;

public class platformcollision : MonoBehaviour
{
    [SerializeField] string PlayerTag = "Player";
    [SerializeField] Transform platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(PlayerTag))
        {
            other.gameObject.transform.parent = platform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(PlayerTag))
        {
            other.gameObject.transform.parent = null;
        }
    }
    
}
