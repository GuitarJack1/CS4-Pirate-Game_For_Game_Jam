using UnityEngine;

public class Tentacle_Face_Player : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Tentacle_Target_Behavior tentTargBehavior;

    [SerializeField]
    private float dontFacePlayerRadius;

    void Update()
    {
        Vector3 flatDistToPlayer = new Vector3(playerTransform.position.x, 0, playerTransform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);

        if (!tentTargBehavior.grabbedPlayer)
            transform.forward = flatDistToPlayer;
    }
}
