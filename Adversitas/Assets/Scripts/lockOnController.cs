using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class lockOnController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public CinemachineVirtualCamera lockOnCamera;
    [SerializeField] public Transform playerCenterOfMass;
    public CinemachineTargetGroup targetGroup;

    [Header("-----Combat Camera Factors-----")]
    [Range(0, 10)][SerializeField] public float targetSwitchSpeed = 2f;
    [Range(0, 150)][SerializeField] public float lockOnDistance = 5f;
    [SerializeField] public float targetWeight;
    [SerializeField] public float targetRadius;
    [SerializeField] public float shoulderSwitchSpeed;

    [Header("-----LockOn-----")]
    public Transform lockOnTarget;
    public bool isLockedOn = false;
    public bool leftShoulder = false;

    private cameraLookController cameraLookController;
    private float targetLockTime = 0f;
    private const float targetLockThreshold = 0.25f;
    private Vector2 targetSwitchInput;
    private bool isSwitching = false;
    private int currentTarget = 0;
    private List<LockOnMe> possibleTargets = new List<LockOnMe>();


    private void Awake()
    {
        playerInput.actions["LockOn"].performed += ctx => ToggleLockOn();
        playerInput.actions["SwitchTarget"].performed += ctx => targetSwitchInput = ctx.ReadValue<Vector2>();
        playerInput.actions["SwitchShoulder"].performed += ctx => ToggleShoulder();
        cameraLookController = GetComponentInParent<cameraLookController>();

        if (targetGroup == null)
        {
            targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            if (targetGroup == null)
            {
                targetGroup = new GameObject("TargetGroup").AddComponent<CinemachineTargetGroup>();
            }
        }
    }

    private void Update()
    {
        SwitchTarget();
        if (isLockedOn)
        {
            targetLockTime += Time.deltaTime;
            Debug.DrawLine(playerCenterOfMass.position, lockOnTarget.position);
            SwitchCameraShoulder();
        }
    }

    public void ToggleLockOn()
    {
        isLockedOn = !isLockedOn;
        Debug.Log("Locked on = " + isLockedOn);

        CinemachineBrain brain = playerCamera.GetComponent<CinemachineBrain>();

        if (isLockedOn)
        {
            targetGroup.AddMember(transform, .35f, 10);
            targetGroup.m_PositionMode = CinemachineTargetGroup.PositionMode.GroupAverage;
            targetGroup.m_RotationMode = CinemachineTargetGroup.RotationMode.GroupAverage;
            targetGroup.m_UpdateMethod = CinemachineTargetGroup.UpdateMethod.FixedUpdate;

            if (lockOnTarget == null)
            {
                FindCameraTarget();            
            }
            if (lockOnCamera != null && brain != null)
            {
                brain.ActiveVirtualCamera.Priority = 10;
                lockOnCamera.LookAt = targetGroup.transform;
            }
        }
        else
        {
            if (lockOnCamera != null && brain != null)
            {
                brain.ActiveVirtualCamera.Priority = 0;
            }
            ResetLockOn();
        }
    }

    public void UpdateCombatCamera()
    {
        if (lockOnTarget != null)
        {
            if (!lockOnCamera.enabled)
            {
                lockOnCamera.enabled = true;
            }
            
            if (!playerCamera.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Equals(lockOnCamera))
            {
                lockOnCamera.Priority = 10;
            }

            Quaternion targetRot = Quaternion.LookRotation(lockOnTarget.position- playerCamera.transform.position);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRot, targetSwitchSpeed);
            lockOnCamera.transform.position = playerCamera.transform.position;
            lockOnCamera.transform.rotation = playerCamera.transform.rotation;
            isSwitching = false;
        }
        else
        {
            cameraLookController.Look();
        }
    }

    public void FindCameraTarget()
    {
        if (isLockedOn && lockOnTarget != null)
        {
            SwitchTarget();
            return;
        }
        possibleTargets.Clear();
        Collider[] targets = Physics.OverlapSphere(playerCenterOfMass.position + Vector3.up, lockOnDistance);
        if (targets.Length <= 0) return;

        for (int i = 0; i < targets.Length; i++)
        {
            GameObject parent = targets[i].gameObject;
            LockOnMe enemy;
            if (parent.TryGetComponent<LockOnMe>(out enemy))
            {
                if (IsTargetVisible(parent.transform))
                {
                    if (!possibleTargets.Contains(enemy))
                    {
                        possibleTargets.Add(enemy);
                    }
                }
                else
                {
                    Debug.Log(enemy + " is obscured and cannot be targeted");
                }
            }
        }

        SortTargetsByDistance();
        if (possibleTargets.Count != 0)
        {
            lockOnTarget = possibleTargets[currentTarget].lockOnPosition;
            possibleTargets[currentTarget].CameraFollowsMe(playerCamera);
            lockOnCamera.Follow = transform;
            targetGroup.AddMember(lockOnTarget, targetWeight, targetRadius);
        }

        if (lockOnTarget == null)
        {
            UpdateCombatCamera();
            ToggleLockOn();
        }
    }

    private bool IsTargetVisible(Transform target)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCenterOfMass.position, target.position, out hit, lockOnDistance))
        {
            if (hit.collider.gameObject.layer != 9)
            {
                Debug.Log("hit object was " + hit.collider.gameObject.name);
                return false;
            }
        }
        return true;
    }

    public void SortTargetsByDistance()
    {
        possibleTargets.Sort((first, second) =>
        {
            float firstDistance = (playerCenterOfMass.position - first.lockOnPosition.position).magnitude;
            float secondDistance = (playerCenterOfMass.position - second.lockOnPosition.position).magnitude;

            return firstDistance.CompareTo(secondDistance);
        });
    }

    public void SwitchTarget()
    {
        if (isSwitching)
        {
            return;
        }

        if (targetLockTime < targetLockThreshold)
        {
            return;
        }

        if (isLockedOn && possibleTargets.Count > 1)
        {
            isSwitching = true;
            if (Mathf.Abs(targetSwitchInput.x) > 0.5f)
            {
                int change = targetSwitchInput.x > 0 ? 1 : -1;
                SwitchTargetIndex(change);
                targetLockTime = 0;
            }
            UpdateCombatCamera();
        }
        targetSwitchInput = Vector2.zero;
    }


    public void SwitchTargetIndex(int change)
    {
        if (isLockedOn && possibleTargets.Count > 1)
        {
            if (change > 0)
            {
                currentTarget = (currentTarget + 1) % possibleTargets.Count;
            }
            else if (change < 0)
            {
                currentTarget = (currentTarget - 1 + possibleTargets.Count) % possibleTargets.Count;
            }

            if (lockOnTarget != possibleTargets[currentTarget].lockOnPosition)
            {
                lockOnTarget = possibleTargets[currentTarget].lockOnPosition;
                lockOnCamera.Follow = transform;
                lockOnCamera.LookAt = lockOnTarget;
                StartCoroutine(LerpTransforms(lockOnCamera.transform, lockOnTarget, targetSwitchSpeed));
            }
        }
    }

    private IEnumerator LerpTransforms(Transform start, Transform end, float duration)
    {
        float timeElapsed = 0f;
        Quaternion startRot = start.rotation;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            start.rotation = Quaternion.Slerp(startRot, end.rotation, t);
            yield return null;
        }
        start.rotation = end.rotation;
    }

    public void ToggleShoulder()
    {
        leftShoulder = !leftShoulder;
    }

    public void SwitchCameraShoulder()
    {
        Cinemachine3rdPersonFollow body;
        if (isLockedOn && leftShoulder)
        {
            body = lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            body.CameraSide = Mathf.Lerp(body.CameraSide, 1, Time.deltaTime * shoulderSwitchSpeed);
        }
        else
        {
            body = lockOnCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            body.CameraSide = Mathf.Lerp(body.CameraSide, 0, Time.deltaTime * shoulderSwitchSpeed);
        }
    }

    public void ResetLockOn()
    {
        lockOnTarget = null;
        possibleTargets.Clear();
        currentTarget = 0;
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

        if (lockOnCamera != null)
        {
            lockOnCamera.Priority = 0;
        }
        cameraLookController.freeLookCamera.Priority = 10;

    }

}
