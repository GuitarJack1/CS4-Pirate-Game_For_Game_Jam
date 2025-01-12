using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot_Gun : MonoBehaviour
{
    public GameObject bullet;
    private Player_Input controls;
    public float reload_time;
    private float next_shoot;

    Vector2 rotation = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        next_shoot = 0.0f;

        controls = new Player_Input();
        controls.Player.Enable();
    }

    private void Shoot()
    {
        if (Time.time > next_shoot)
        {
            next_shoot = Time.time + reload_time;
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.Player.Shoot.IsPressed())
        {
            Shoot();
        }
    }
}
