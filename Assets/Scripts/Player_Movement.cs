using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Player_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float speedForce = 40f;
    [SerializeField]
    private float maxSpeed = 15f;
    [SerializeField]
    private float jumpHeight = 4f;
    [SerializeField]
    private float fallMultiplier = 3f;
    [SerializeField]
    private float lowJumpMultiplier = 2f;
    [SerializeField]
    private float groundDrag = 3f;
    [SerializeField]
    private float airDrag = 4f;

    [Header("Interaction Settings")]
    [SerializeField]
    private LayerMask groundMask;

    [Header("Game Objects")]
    [SerializeField]
    private Camera playerCamera;

    [Header("Camera Settings")]
    [SerializeField]
    private float lookSpeed = 0.5f;
    [SerializeField]
    private float upDownLimit = 90f;

    private Player_Input controls;
    public Rigidbody rb;
    Vector2 rotation = Vector2.zero;
    [SerializeField]
    private bool grounded = false;

    public float mouseY;

    Shoot_Gun shootGunScript;

    private bool grabbed;

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

        shootGunScript = GetComponentInChildren<Shoot_Gun>();
        GetComponent<AudioSource>().Pause();
    }

    void Update()
    {
        Vector2 mouseVector = controls.Player.Mouse.ReadValue<Vector2>();
        mouseVector.y += shootGunScript.recoilOffset;
        shootGunScript.recoilOffset = 0;

        // Player and Camera rotation
        rotation.x += -mouseVector.y * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -upDownLimit, upDownLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);

        Quaternion localRotation = Quaternion.Euler(0f, mouseVector.x * lookSpeed, 0f);
        transform.rotation = transform.rotation * localRotation;

        if (!grabbed)
        {
            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0f && !controls.Player.Jump.IsPressed())
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
            }

            if (controls.Player.Movement.ReadValue<Vector2>() != new Vector2(0, 0))
            {
                GetComponent<AudioSource>().UnPause();
            }
            else
            {
                GetComponent<AudioSource>().Pause();
            }

            SpeedLimit();
        }
    }

    void FixedUpdate()
    {
        if (!grabbed)
        {
            Vector2 movementInputVector = controls.Player.Movement.ReadValue<Vector2>();
            Vector3 movementDirection = transform.forward * movementInputVector.y + transform.right * movementInputVector.x;

            rb.AddForce(movementDirection.normalized * speedForce, ForceMode.Acceleration);

            if (grounded)
            {
                rb.linearDamping = groundDrag;

                if (controls.Player.Jump.IsPressed() && grounded)
                {
                    rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
                    grounded = false;
                }
            }
            else
            {
                rb.linearDamping = airDrag;
            }
        }
    }

    void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 newVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(newVel.x, rb.linearVelocity.y, newVel.z);
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (groundMask == (groundMask | (1 << collision.gameObject.layer)))
        {
            grounded = true;
        }
    }

    public void Grabbed()
    {
        rb.isKinematic = true;
    }
    public void Dropped()
    {
        rb.isKinematic = false;
    }
}
