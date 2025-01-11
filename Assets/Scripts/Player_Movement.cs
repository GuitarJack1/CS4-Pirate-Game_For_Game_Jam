using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Player_Movement : MonoBehaviour
{
    public float speedForce = 15.0f;
    public float maxSpeed = 15.0f;
    public float jumpHeight = 2.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float upDownLimit = 60.0f;
    public float groundDrag = 0;
    private Player_Input controls;

    public bool grounded = false;
    Rigidbody rb;
    Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rotation.y = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new Player_Input();
        controls.Player.Enable();
    }

    void Update()
    {
        Vector2 mouseVector = controls.Player.Mouse.ReadValue<Vector2>();

        // Player and Camera rotation
        rotation.x += -mouseVector.y * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -upDownLimit, upDownLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);

        Quaternion localRotation = Quaternion.Euler(0f, mouseVector.x * lookSpeed, 0f);
        transform.rotation = transform.rotation * localRotation;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            rb.linearDamping = groundDrag;

            Vector2 movementInputVector = controls.Player.Movement.ReadValue<Vector2>();
            Vector3 movementDirection = transform.forward * movementInputVector.y + transform.right * movementInputVector.x;

            rb.AddForce(movementDirection.normalized * speedForce);

            if (controls.Player.Jump.IsPressed() && grounded)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
                grounded = false;
            }
        }
        else
        {
            rb.linearDamping = 0;
        }

        SpeedLimit();
    }

    void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 newVel = flatVel.normalized * speedForce;
            rb.linearVelocity = new Vector3(newVel.x, rb.linearVelocity.y, newVel.z);
        }

    }

    void OnCollisionStay()
    {
        grounded = true;
    }
}
