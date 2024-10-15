using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public CinemachineFreeLook freeLookCamera;
    [SerializeField] public Transform centerOfMass;
    [SerializeField] public Transform defaultCameraPosition;

    [Header("-----Normal Camera Factors-----")]
    [Range(0, 4)][SerializeField] public float lookSpeed = 2f;
    [Range(0, 25)][SerializeField] public float distanceFromPlayer = 5f;
    [Range(0, 15)][SerializeField] public float height = 2f;
    [Range(0, 200)][SerializeField] public float pitchLimit = 80f;
    [Range(0, 25)][SerializeField] public float turnSpeed = 2f;


    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private lockOnController lockOnController;


    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        lockOnController = GetComponentInParent<lockOnController>();
        freeLookCamera.transform.position = defaultCameraPosition.position;
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
    }

    public void Look()
    {
        RotateCamera();
        UpdateVirtualCamera();
    }

    void RotateCamera()
    {
        currentYaw += lookInput.x * lookSpeed;
        currentPitch -= lookInput.y * lookSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);
    }

    public void UpdateVirtualCamera()
    {
        if (!freeLookCamera.enabled)
        {
            freeLookCamera.enabled = true;
        }

        CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
        if (brain != null && freeLookCamera != null)
        {
            freeLookCamera.Priority = 10;
        }
        //Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);
        //freeLookCamera.transform.rotation = rotation;
        //freeLookCamera.transform.position = centerOfMass.position + rotation * new Vector3(0, height, -distanceFromPlayer);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        freeLookCamera.transform.rotation = rotation;
        freeLookCamera.transform.position = centerOfMass.position + rotation * new Vector3(0, height, -distanceFromPlayer);
    
    }

}
