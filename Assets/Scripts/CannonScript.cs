using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class CannonScript : MonoBehaviour
{
    private Player_Input controls;

    [SerializeField]
    private Shoot_Gun shootGunScript;
    [SerializeField]
    private PopupScript popupScript;
    public GameObject eButton;

    public Camera playerCamera;
    public Camera cannonCamera;
    public GameObject ammoUI;

    [SerializeField]
    private GameObject cannonCameraGO;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnTransform;

    [SerializeField]
    private float bulletForce;
    public float shoot_time;
    private float next_shoot;

    [Header("Camera Settings")]
    [SerializeField]
    private float lookSpeed = 0.5f;
    [SerializeField]
    private float upDownLimit = 40f;
    [SerializeField]
    private float leftRightLimit = 50f;

    Vector2 rotation = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        next_shoot = 0.0f;

        controls = new Player_Input();
        controls.Player.Enable();
        controls.Cannon.Enable();
        
        controls.Player.pressE.performed += pressE;
        controls.Cannon.pressE.performed += pressE;

        controls.Cannon.Disable();

        cannonCamera.enabled = false;
    }

    void pressE(CallbackContext cb){
        if(playerCamera.enabled && ((playerCamera.transform.position - eButton.transform.position).magnitude < popupScript.appearDistance)){
            controls.Player.Disable();
            shootGunScript.controls.Player.Disable();
            controls.Cannon.Enable();
            playerCamera.enabled = !playerCamera.enabled;
            cannonCamera.enabled = !cannonCamera.enabled;
            ammoUI.SetActive(false);
        } else if(cannonCamera.enabled) {
            controls.Player.Enable();
            shootGunScript.controls.Player.Enable();
            controls.Cannon.Disable();
            playerCamera.enabled = !playerCamera.enabled;
            cannonCamera.enabled = !cannonCamera.enabled;
            ammoUI.SetActive(true);
        }
    }

    private void Shoot()
    {
        if (Time.time > next_shoot)
        {
            next_shoot = Time.time + shoot_time;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletForce, ForceMode.VelocityChange);
            bullet.GetComponent<LookAtCamera>().mainCamera = cannonCameraGO;
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.Cannon.Shoot.IsPressed())
        {
            Shoot();
        }

        Vector2 mouseVector = controls.Cannon.Mouse.ReadValue<Vector2>();
        rotation.x += -mouseVector.y * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -upDownLimit, upDownLimit);
        rotation.y += mouseVector.x * lookSpeed;
        rotation.y = Mathf.Clamp(rotation.y, -leftRightLimit, leftRightLimit);
        cannonCamera.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
    }
}
