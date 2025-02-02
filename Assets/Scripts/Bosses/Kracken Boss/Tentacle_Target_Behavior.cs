using UnityEngine;

public class Tentacle_Target_Behavior : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform targetDefaultPos;

    [SerializeField]
    private float attackPlayerRadius;
    [SerializeField]
    private float tentacleSpeed;
    [SerializeField]
    private float stunnedTime;

    [SerializeField]
    private float grabbedTentacleSpeed;

    [HideInInspector]
    public bool grabbedPlayer;

    [SerializeField]
    private float timeToThrow;
    [SerializeField]
    private float throwForce;

    private float grabbedStartTime = 0f;

    private bool stunned;
    private GameObject player;

    private float stunnedStartTime = 0f;
    private float throwTime = 0f;

    void Start()
    {
        grabbedPlayer = false;
        stunned = false;
    }

    void Update()
    {
        Vector3 flatDistToPlayer = new Vector3(targetDefaultPos.position.x, 0, targetDefaultPos.position.z) - new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        if (flatDistToPlayer.magnitude < attackPlayerRadius && !grabbedPlayer && !stunned)
        {
            Vector3 towardsPlayer = playerTransform.position - transform.position;
            transform.position += towardsPlayer.normalized * tentacleSpeed;
        }
        else
        {
            Vector3 towardsDefaultPos = targetDefaultPos.position - transform.position;
            transform.position += towardsDefaultPos.normalized * (grabbedPlayer ? grabbedTentacleSpeed : tentacleSpeed);
        }

        if (Time.time >= stunnedStartTime + stunnedTime)
        {
            stunned = false;
        }

        if (stunned && grabbedPlayer)
        {
            player.transform.parent = null;
            player.GetComponent<Player_Movement>().Dropped();
            grabbedPlayer = false;
        }

        if (grabbedPlayer && Time.time > grabbedStartTime + timeToThrow)
        {
            player.transform.parent = null;
            player.GetComponent<Player_Movement>().Dropped();
            grabbedPlayer = false;

            Vector3 randomThrow = Random.onUnitSphere;
            player.GetComponent<Player_Movement>().rb.AddForce(new Vector3(randomThrow.x, Mathf.Abs(randomThrow.y), randomThrow.z) * throwForce, ForceMode.VelocityChange);

            throwTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time > throwTime + 1 && other.gameObject.layer == 6)
        {
            other.gameObject.transform.parent = transform;
            other.gameObject.GetComponent<Player_Movement>().Grabbed();
            player = other.gameObject;
            grabbedPlayer = true;

            grabbedStartTime = Time.time;
        }
    }

    public void Hit()
    {
        stunned = true;
        stunnedStartTime = Time.time;
    }
}
