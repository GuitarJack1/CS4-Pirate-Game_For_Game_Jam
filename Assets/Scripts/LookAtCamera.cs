using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject mainCamera;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
    }
}
