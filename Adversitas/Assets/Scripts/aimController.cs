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
    public bool isAiming = false;
    public bool leftShoulder = false;

    public Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;
    private lockOnController lockOnController;
    private cameraLookController cameraLookController;
    private movementController moveController;
    private Animator animator;



    void Awake()
    {
        playerInput.actions["Aim"].performed += ctx => OnHoldButton(ctx);
        playerInput.actions["Aim"].canceled += ctx => OnReleaseButton(ctx);
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        lockOnController = GetComponentInParent<lockOnController>();
        cameraLookController = GetComponentInParent<cameraLookController>();
        moveController = GetComponentInParent<movementController>();

        animator = GetComponentInParent<Animator>();

        uiCanvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Canvas>();
        aimImage = uiCanvas.GetComponentInChildren<Image>();
    }



    void Update()
    {
        Debug.DrawLine(headPos.position, headPos.transform.forward * 100, Color.blue);

        if (isAiming)
        {
            //SwitchCameraShoulder();
            GetComponentInParent<Animator>().SetBool("IsAiming", isAiming);
        }
        else
        {
            aimImage.enabled = false;
        }
        Debug.DrawLine(aimCamera.transform.position, aimCamera.transform.forward * aimDistance, Color.blue);
        Debug.DrawLine(headPos.position, aimCamera.transform.forward *aimDistance, Color.green);
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
            animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
            CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
            brain.ActiveVirtualCamera.Priority = 0;
            aimCamera.Priority = 10;
            UpdateAim();
            Debug.DrawLine(aimCamera.transform.position, aimCamera.transform.forward * aimDistance, Color.blue);
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

        if (!aimImage.enabled)
        {
            aimImage.enabled = true;
        }
        RaycastHit hit;
        Physics.Raycast(headPos.position, aimCamera.transform.forward, out hit, aimDistance);
        if (hit.collider != null)
        {
            Debug.Log("Ray cast hit " + hit.collider.name);

            LockOnMe target = hit.collider.gameObject.GetComponent<LockOnMe>();
            if (target != null)
            {
                Debug.Log("Enemy touched");
                aimCamera.LookAt = target.lockOnPosition;
            }
        }
    }

    void RotateCamera()
    {
        currentYaw += lookInput.x * aimSpeed;
        currentPitch -= lookInput.y * aimSpeed;
        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        Quaternion rotation = Quaternion.Euler(currentYaw, currentPitch, 0);
        aimCamera.transform.rotation = rotation;
        aimCamera.transform.position = (centerOfMass.position + rotation * new Vector3(0, 0, 0));

    }

    private void OnReleaseButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isAiming = false;
            GetComponentInParent<Animator>().SetBool("IsAiming", isAiming);
            animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
            CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();
            brain.ActiveVirtualCamera.Priority = 0;
            cameraLookController.lookCamera.Priority = 10;
            ResetAim();
        }
    }

    public void ResetAim()
    {
        aimImage.enabled = false;
    }

    //public void ToggleShoulder()
    //{
    //    if (isAiming)
    //    {
    //        leftShoulder = !leftShoulder;
    //    }
    //}

    //public void SwitchCameraShoulder()
    //{
    //    if (isAiming)
    //    {
    //        Cinemachine3rdPersonFollow body;
    //        if (leftShoulder)
    //        {
    //            body = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    //            body.CameraSide = Mathf.Lerp(body.CameraSide, 0, Time.deltaTime * shoulderSwitchSpeed);
    //            aimTarget.position = aimCamera.transform.forward * aimDistance;
    //            aimCamera.LookAt = aimTarget;

    //        }
    //        else
    //        {
    //            body = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    //            body.CameraSide = Mathf.Lerp(body.CameraSide, 1, Time.deltaTime * shoulderSwitchSpeed);
    //            aimTarget.position = aimCamera.transform.forward * aimDistance;
    //            aimCamera.LookAt = aimTarget;
    //        }
    //    }
    //    else
    //    {
    //        aimImage.enabled = false;
    //    }
    //}

    //public void AdjustRotationSpeed()
    //{
    //    if (isAiming)
    //    {
    //        movementController
    //    }
    //}
}
