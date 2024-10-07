using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;

    [Header("-----Camera Factors-----")]
    [Range(0, 5)][SerializeField] float cameraLookSpeed = 2f; // Speed of looking around
    [Range(0, 5)][SerializeField] float distanceFromPlayer = 5f; // Distance of camera from the player
    [Range(0, 5)][SerializeField] float height = 2f; // Height of the camera above the player
    [Range(0, 200)][SerializeField] float pitchLimit = 80f; // Limit for pitch to prevent camera flipping
    [Range(0, 5)][SerializeField] public float playerTurnToCameraSpeed = 2f; // Speed for lerping toward camera direction

    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;


    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
    }

    void Update()
    {
        Look();
    }

    public void Look()
    {
        RotateCamera();
        UpdateCameraTransform();
    }

    void RotateCamera()
    {
        currentYaw += lookInput.x * cameraLookSpeed;
        currentPitch -= lookInput.y * cameraLookSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);
    }

    void UpdateCameraTransform()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer) + new Vector3(0, height, 0);
        playerCamera.transform.position = transform.position + offset;

        playerCamera.transform.LookAt(transform.position + Vector3.up * height);
        Physics.SyncTransforms();
    }
}
