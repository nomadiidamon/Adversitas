using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class controllableEntity: MonoBehaviour, IMove, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Rigidbody rb;

    [Header("-----Camera-----")]
    [SerializeField] Camera playerCamera;
    [Range(0, 5)][SerializeField] float cameraLookSpeed = 2f; // Speed of looking around
    [Range(0, 5)][SerializeField] float distanceFromPlayer = 5f; // Distance of camera from the player
    [Range(0, 5)][SerializeField] float height = 2f; // Height of the camera above the player
    [Range(0, 200)][SerializeField] float pitchLimit = 80f; // Limit for pitch to prevent flipping
    [Range(0, 5)][SerializeField] float playerTurnToCameraSpeed = 2f; // Speed for lerping toward camera direction


    [Header("-----Attributes-----")]
    [Range(0, 20)][SerializeField] float speed;


    private Vector2 moveInput;
    private Vector2 lookInput;
    private float currentYaw; // Keep track of the current yaw for orbiting
    private float currentPitch; // Keep track of the current pitch for up/down rotation



    void Awake()
    {
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
    }

    void Update()
    {
        Look();
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        // Calculate movement direction based on the camera's orientation
        Vector3 cameraForward = playerCamera.transform.forward; // Get the camera's forward direction
        cameraForward.y = 0; // Ignore vertical movement
        cameraForward.Normalize(); // Normalize the vector to ensure consistent movement speed

        Vector3 right = playerCamera.transform.right; // Get the camera's right direction
        right.y = 0; // Ignore vertical movement
        right.Normalize(); // Normalize the vector

        // Combine movement input with camera's forward and right vectors
        Vector3 movement = (cameraForward * moveInput.y + right * moveInput.x).normalized;

        float currentSpeed = speed; // You can modify this for sprinting if needed
                                    // Move the player
        rb.position += movement * currentSpeed * Time.deltaTime;

        if (this.transform.rotation != playerCamera.transform.rotation)
        {
            Quaternion targetRotation = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerTurnToCameraSpeed);
        }
    }

    public void Look()
    {
        // Update the current yaw based on look input
        currentYaw += lookInput.x * cameraLookSpeed;
        currentPitch -= lookInput.y * cameraLookSpeed; // Invert pitch for up/down

        // Clamp the pitch to prevent flipping
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        // Calculate the new camera position
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer) + new Vector3(0, height, 0);
        playerCamera.transform.position = transform.position + offset;

        // Make the camera look at the player
        playerCamera.transform.LookAt(transform.position + Vector3.up * height); // Adjusted to look at player's height
    }

}
