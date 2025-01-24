using UnityEngine;

public class PopupScript : MonoBehaviour
{
    public GameObject mainCamera;
    public float appearDistance;
    public GameObject ePanel;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
        if((mainCamera.transform.position - transform.position).magnitude < appearDistance){
            ePanel.SetActive(true);
        }else{
            ePanel.SetActive(false);
        }
    }
}