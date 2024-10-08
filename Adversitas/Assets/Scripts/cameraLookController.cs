using UnityEngine;
using UnityEngine.InputSystem;


public class cameraLookController : MonoBehaviour, ILook
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;

    [Header("-----Normal Camera Factors-----")]
    [SerializeField] public cameraSettings normalCameraSettings;

    [Header("-----Combat Camera Factors-----")]
    [SerializeField] public cameraSettings combatCameraSettings;

    [Header("-----Camera Mode Change Speed-----")]
    [SerializeField] public float cameraChangeSpeed;

    [Header("-----LockOn-----")]
    [SerializeField] private Transform lockOnTarget; // The target to lock onto
    [SerializeField] private float lockOnDistance = 5f; // Distance from the target when locked on
    private bool isLockedOn = false; // Whether the camera is currently locked on

    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private Transform playerPosition;
    private cameraSettings settings;



    void Awake()
    {
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions["LockOn"].performed += ctx => ToggleLockOn();

        playerPosition = playerManager.instance.player.transform;
        settings = normalCameraSettings;
    }

    void Update()
    {
        if (!isLockedOn)
        {
            Look();
        }
        else
        {
            if (lockOnTarget == null)
            {
                FindCameraTarget();
            }
           UpdateCombatCamera();
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

    public void UpdateCombatCamera()
    {
        //Quaternion lockOnRotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        //Vector3 lockOnOffset = lockOnRotation * new Vector3(0, 0, -settings.distanceFromPlayer) + new Vector3(0, settings.height, 0);
        ////playerCamera.transform.position = playerCamera.transform.position + lockOnOffset;

        if (lockOnTarget != null)
        {
            playerCamera.transform.LookAt(lockOnTarget.position + Vector3.up * settings.height);
        }
        else
        {
            playerCamera.transform.LookAt(transform.position + Vector3.up * settings.height);
        }
    }

    public void ToggleLockOn()
    {
        isLockedOn = !isLockedOn;
        Debug.Log("Locked on = " + isLockedOn);

        if (isLockedOn)
        {

            if (lockOnTarget == null)
            {
                FindCameraTarget();
                //CombatCameraOn();
            }   

        }
        else
        {
            lockOnTarget = null;
            NormalCameraOn();
        }
    }

    public void FindCameraTarget()
    {
        Collider[] targets = Physics.OverlapSphere(playerPosition.position, lockOnDistance);

        if (targets.Length <= 0)
        {
            return;
        }

        if (isLockedOn && lockOnTarget != null)
        {
            SwitchTarget();
            return;
        }

        Debug.Log("Searching for targets");
        for (int i = 0; i < targets.Length; i++)
        {
            GameObject parent = targets[i].gameObject;
            LockOnMe enemy;
            if (parent.TryGetComponent<LockOnMe>(out enemy)) {
                enemy.CameraFollowsMe(playerCamera);
                lockOnTarget = enemy.lockOnPosition;
                CombatCameraOn();
                UpdateCombatCamera();
                Debug.Log(enemy + " is the new target");
                return;
            }

        }
    }

    public void SwitchTarget()
    {

    }

    public void NormalCameraOn()
    {
        settings = normalCameraSettings;
        Rigidbody rb = playerCamera.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Physics.SyncTransforms();
    }

    public void CombatCameraOn()
    {
        settings = combatCameraSettings;
        Rigidbody rb = playerCamera.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Physics.SyncTransforms();
    }


}
