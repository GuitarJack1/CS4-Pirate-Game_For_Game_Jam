using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class CannonScript : MonoBehaviour
{
    private Player_Input controls;

    public Camera playerCamera;
    public Camera cannonCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new Player_Input();
        controls.Player.Enable();
        
        controls.Player.pressE.performed += pressE;
    }

    void pressE(CallbackContext cb){
        playerCamera.enabled = !playerCamera.enabled;
        cannonCamera.enabled = !cannonCamera.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
