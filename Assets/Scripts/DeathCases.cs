using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCases : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
           
            SceneManager.LoadScene("MainMenuScene");
            Debug.Log("I should be DEAD");
        }
    }
}