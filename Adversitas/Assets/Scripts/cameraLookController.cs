using UnityEngine;
using UnityEngine.InputSystem;


public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public Transform centerOfMass { get; set; }


    [Header("-----Normal Camera Factors-----")]
    [SerializeField] public cameraSettings normalCameraSettings;

    [Header("-----Combat Camera Factors-----")]
    [SerializeField] public cameraSettings combatCameraSettings;
    [SerializeField] public float lockOnDistance = 5f; // Distance from the target when locked on

    [Header("-----Camera Mode Change Speed-----")]
    [SerializeField] public float cameraChangeSpeed;

    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private Transform playerPosition;
    private cameraSettings settings;
    private lockOnController lockOnController;



    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions["LockOn"].performed += ctx => lockOnController.ToggleLockOn();

        playerPosition = playerManager.instance.player.transform;
        settings = normalCameraSettings;

        lockOnController = gameObject.AddComponent<lockOnController>();
        lockOnController.Initialize(playerPosition, playerCamera, lockOnDistance, normalCameraSettings);
    }

    void Update()
    {
        if (!lockOnController.isLockedOn)
        {
            Look();
        }
        else
        {
            lockOnController.UpdateCombatCamera(settings);
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
        currentYaw += lookInput.x * settings.lookSpeed;
        currentPitch -= lookInput.y * settings.lookSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -settings.pitchLimit, settings.pitchLimit);
    }

    public void UpdateNormalCamera()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -settings.distanceFromPlayer) + new Vector3(0, settings.height, 0);
        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * settings.height);
    }
}
