using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class aimController : MonoBehaviour
{

    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public CinemachineVirtualCamera aimCamera;
    [SerializeField] public Transform centerOfMass;
    [SerializeField] public Transform headPos;
    [SerializeField] public Transform aimTarget;


    [Header("-----Aim Factors-----")]
    [SerializeField] public float shoulderSwitchSpeed;
    [SerializeField] public float aimSpeed;
    [Range(0, 200)][SerializeField] public float pitchLimit = 50f;
    [SerializeField] public float aimDistance;

    private Canvas uiCanvas;
    private Image aimImage;
    private CinemachinePOV aimPov;
    public bool isAiming = false;
    public bool leftShoulder = false;

    public Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private lockOnController lockOnController;
    private cameraLookController cameraLookController;
    private movementController moveController;

    private Transform originalTargetPos;


    void Awake()
    {
        playerInput.actions["Aim"].performed += ctx => OnHoldButton(ctx);
        playerInput.actions["Aim"].canceled += ctx => OnReleaseButton(ctx);
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions["SwitchShoulder"].performed += ctx => ToggleShoulder();

        lockOnController = GetComponentInParent<lockOnController>();
        cameraLookController = GetComponentInParent<cameraLookController>();
        moveController = GetComponentInParent<movementController>();

        //originalTargetPos = aimTarget.transform;
        uiCanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
        aimImage = uiCanvas.GetComponentInChildren<Image>();
        aimPov = aimCamera.GetCinemachineComponent<CinemachinePOV>();
    }



    void Update()
    {
        Debug.DrawLine(headPos.position, headPos.transform.forward * 100, Color.blue);

        if (isAiming)
        {
            SwitchCameraShoulder();
        }
        else
        {
            aimImage.enabled = false;
        }
    }

    private void OnHoldButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (lockOnController.isLockedOn)
            {
                return;
            }
            isAiming = true;
            CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
            brain.ActiveVirtualCamera.Priority = 0;
            aimCamera.Priority = 10;
            UpdateAim();
        }
    }

    public void UpdateAim()
    {
        if (isAiming)
        {
            UpdateAimTargetPosition();
        }
    }

    public void UpdateAimTargetPosition()
    {
        RotateCamera();
        UpdateAimPOV();
        if (lookInput == Vector2.zero)
        {
            aimPov.m_VerticalAxis.Value = 0;
            aimPov.m_HorizontalAxis.Value = 0;
        }
        else
        {
            aimPov.m_VerticalAxis.Value = lookInput.y;
            aimPov.m_HorizontalAxis.Value = lookInput.x;
        }

        if (!aimImage.enabled)
        {
            aimImage.enabled = true;
        }
        RaycastHit hit;
        Physics.Raycast(headPos.position, aimCamera.transform.forward, out hit, 100f);
        if (hit.collider != null)
        {
            Debug.Log("Ray cast hit " + hit.collider.name);

            LockOnMe target = hit.collider.gameObject.GetComponent<LockOnMe>();
            if (target != null)
            {
                Debug.Log("Enemy touched");
                aimPov.m_VerticalAxis.Value = 0;
                aimPov.m_HorizontalAxis.Value = 0;
                aimCamera.LookAt = target.lockOnPosition;
            }
        }
    }

    private void UpdateAimPOV()
    {
        if (lookInput == Vector2.zero)
        {
            aimPov.m_VerticalAxis.Value = 0;
            aimPov.m_HorizontalAxis.Value = 0;
        }
        else
        {
            aimPov.m_VerticalAxis.Value = Mathf.Lerp(aimPov.m_VerticalAxis.Value, lookInput.y, Time.deltaTime * aimSpeed);
            aimPov.m_HorizontalAxis.Value = Mathf.Lerp(aimPov.m_HorizontalAxis.Value, lookInput.x, Time.deltaTime * aimSpeed);
        }
    }

    void RotateCamera()
    {
        //currentYaw += lookInput.x * aimSpeed;
        //currentPitch -= lookInput.y * aimSpeed;
        ////currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        //Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        //aimCamera.transform.rotation = rotation;
        //aimCamera.transform.position = (centerOfMass.position + rotation *new Vector3(0,0,0));


        currentYaw = Mathf.Clamp(currentYaw + lookInput.x * aimSpeed, -360, 360);
        currentPitch = Mathf.Clamp(currentPitch - lookInput.y * aimSpeed, -pitchLimit, pitchLimit);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        aimCamera.transform.rotation = rotation;
        aimCamera.transform.position = centerOfMass.position; // Simplified, you can adjust as needed.
    }

    private void OnReleaseButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isAiming = false;
            CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
            brain.ActiveVirtualCamera.Priority = 0;
            cameraLookController.freeLookCamera.Priority = 10;
            ResetAim();
        }
    }

    public void ResetAim()
    {
        aimImage.enabled = false;
    }

    public void ToggleShoulder()
    {
        if (isAiming)
        {
            leftShoulder = !leftShoulder;
        }
    }

    public void SwitchCameraShoulder()
    {
        if (isAiming)
        {
            Cinemachine3rdPersonFollow body;
            if (leftShoulder)
            {
                body = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                body.CameraSide = Mathf.Lerp(body.CameraSide, 0, Time.deltaTime * shoulderSwitchSpeed);
                aimTarget.position = aimCamera.transform.forward * aimDistance;
                aimCamera.LookAt = aimTarget;

            }
            else
            {
                body = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                body.CameraSide = Mathf.Lerp(body.CameraSide, 1, Time.deltaTime * shoulderSwitchSpeed);
                aimTarget.position = aimCamera.transform.forward * aimDistance;
                aimCamera.LookAt = aimTarget;
            }
        }
        else
        {
            aimImage.enabled = false;
        }
    }

    //public void AdjustRotationSpeed()
    //{
    //    if (isAiming)
    //    {
    //        movementController
    //    }
    //}
}
