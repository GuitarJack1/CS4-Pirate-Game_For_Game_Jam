using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using TMPro;

public class Shoot_Gun : MonoBehaviour
{
    public GameObject bullet;
    private Player_Input controls;
    public float shoot_time;
    private float next_shoot;
    private float fakeAmmo;
    public int ammo;
    public int maxAmmo = 30;
    public TMP_Text text;
    public float reload_time;
    private float last_reload_time;
    private bool reloading;

    Vector2 rotation = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reloading = false;
        next_shoot = 0.0f;
        ammo = maxAmmo;

        controls = new Player_Input();
        controls.Player.Enable();
        controls.Player.Reload.performed += Reload;
    }

    private void Shoot()
    {
        if (Time.time > next_shoot && ammo > 0)
        {
            reloading = false;
            ammo--;
            next_shoot = Time.time + shoot_time;
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    void Reload(CallbackContext cb){
        last_reload_time = Time.time;
        fakeAmmo = ammo;
        reloading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ammo >= maxAmmo){
            reloading = false;
        }
        if (controls.Player.Shoot.IsPressed())
        {
            Shoot();
        }
        text.text = ammo + "/" + maxAmmo;
        if(Time.time < last_reload_time + reload_time && ammo < maxAmmo && reloading){
            fakeAmmo += maxAmmo/reload_time * Time.deltaTime;
            ammo = (int)fakeAmmo;
        }
    }
}
