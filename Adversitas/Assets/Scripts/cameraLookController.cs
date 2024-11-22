using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public CinemachineVirtualCamera lookCamera;
    [SerializeField] public Transform centerOfMass;
    [SerializeField] public Transform bodyRotatePoint;


    [Header("-----Normal Camera Factors-----")]
    [Range(0, 4)][SerializeField] public float lookSpeed = 2f;
    [Range(0, 25)][SerializeField] public float distanceFromPlayer = 5f;
    [Range(0, 15)][SerializeField] public float height = 2f;
    [Range(0, 200)][SerializeField] public float pitchLimit = 80f;
    [Range(0, 25)][SerializeField] public float turnSpeed = 2f;


    public Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private lockOnController lockOnController;
    private aimController aimController;
    private movementController movementController;
    public bool isLooking = false;


    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        lockOnController = GetComponentInParent<lockOnController>();
        aimController = GetComponentInParent<aimController>();
        movementController = GetComponentInParent<movementController>();
    }

    void Update()
    {
            if (lockOnController == null || !lockOnController.isLockedOn)
            {
                Look();
            }
    }

    public void Look()
    {
        isLooking = true;
        RotateCamera();
        UpdateVirtualCamera();
        isLooking = false;
    }

    void RotateCamera()
    {
        currentYaw += lookInput.x * lookSpeed;
        currentPitch -= lookInput.y * lookSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);
    }

    public void UpdateVirtualCamera()
    {
        CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
        if (brain != null && lookCamera != null && lookCamera.Priority != 10)
        {
            lookCamera.Priority = 10;
        }

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        lookCamera.transform.rotation = rotation;
        lookCamera.transform.up = centerOfMass.up;

        bodyRotatePoint.rotation = rotation;
        transform.rotation = rotation;
    }

}

    

