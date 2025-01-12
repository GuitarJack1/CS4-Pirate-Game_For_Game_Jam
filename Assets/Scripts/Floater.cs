using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    
    private void Start()
    {
        Vector3 position = transform.position;
        position.y = WaveManager.instance.GetWaveHeight(position.x, position.z);
        transform.position = position;
    }
    private void FixedUpdate()
    {
    
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x, transform.position.z);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y )/ depthBeforeSubmerged) * displacementAmount;

            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(-rigidBody.velocity.normalized * waterDrag * rigidBody.velocity.sqrMagnitude * displacementMultiplier * Time.fixedDeltaTime, ForceMode.Force);
            rigidBody.AddTorque(-rigidBody.angularVelocity * waterAngularDrag * displacementMultiplier * Time.fixedDeltaTime, ForceMode.Force);
        }

        
    }
}