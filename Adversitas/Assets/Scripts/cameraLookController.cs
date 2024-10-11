using UnityEngine;
using UnityEngine.InputSystem;


public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public Transform centerOfMass;
    [SerializeField] public Transform defaultCameraPosition;

    [Header("-----Normal Camera Factors-----")]
    [SerializeField] public cameraSettings normalCameraSettings;

    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private lockOnController lockOnController;


    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        lockOnController = GetComponentInParent<lockOnController>();
        //playerCamera.transform.position = defaultCameraPosition.position;
    }

    void Update()
    {
        if (!lockOnController.isLockedOn)
        {
            Look();
        }
        else
        {
            lockOnController.UpdateCombatCamera();
        }
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward *50, Color.green);
    }

    public void Look()
    {
        RotateCamera();
        UpdateNormalCamera();
    }

    void RotateCamera()
    {
        currentYaw += lookInput.x * normalCameraSettings.lookSpeed;
        currentPitch -= lookInput.y * normalCameraSettings.lookSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -normalCameraSettings.pitchLimit, normalCameraSettings.pitchLimit);
    }

    public void UpdateNormalCamera()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -normalCameraSettings.distanceFromPlayer) + new Vector3(0, normalCameraSettings.height, 0);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * normalCameraSettings.height);
    }    

}
