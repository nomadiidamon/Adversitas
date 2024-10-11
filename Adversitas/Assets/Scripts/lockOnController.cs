using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class lockOnController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public Transform lockedOnCameraPosition;
    [SerializeField] public Transform lockedOnCameraPositionInverse;
    [SerializeField] public Transform playerCenterOfMass;


    [Header("-----Combat Camera Factors-----")]
    [Range(0, 2)][SerializeField] public float targetSwitchSpeed = 2f;
    [Range(0, 15)][SerializeField] public float distanceFromPlayer = 5f;
    [Range(0, 15)][SerializeField] public float height = 2f;
    [Range(0, 200)][SerializeField] public float pitchLimit = 80f;
    [Range(0, 25)][SerializeField] public float turnSpeed = 2f;
    [SerializeField] public float lockOnDistance = 5f; // Distance from the target when locked on


    [Header("-----LockOn-----")]
    public Transform lockOnTarget; // The target to lock onto
    public bool isLockedOn = false; // Whether the camera is currently locked on

    private cameraLookController cameraLookController;
    private Vector2 targetSwitchInput;
    private bool isSwitching = false;
    public bool leftShoulder = false;


    private int currentTarget = 0;

    private List<LockOnMe> possibleTargets = new List<LockOnMe>();



    private void Awake()
    {
        playerInput.actions["LockOn"].performed += ctx => ToggleLockOn();
        playerInput.actions["SwitchTarget"].performed += ctx => targetSwitchInput = ctx.ReadValue<Vector2>();
        playerInput.actions["SwitchShoulder"].performed += ctx => ToggleShoulder();
        cameraLookController = GetComponentInParent<cameraLookController>();

    }


    private void Update()
    {
        SwitchTarget();
        if (isLockedOn)
        {
            Debug.DrawLine(playerCenterOfMass.position, lockOnTarget.position);
            SwitchCameraShoulder();
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
            }
        }
        else
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraLookController.defaultCameraPosition.position, cameraLookController.turnSpeed * Time.deltaTime);
            ResetLockOn();
        }
    }

    public void UpdateCombatCamera()
    {
        if (lockOnTarget != null)
        {
            playerCamera.transform.position = lockedOnCameraPosition.position;
            Quaternion targetRot = Quaternion.LookRotation(lockOnTarget.position- playerCamera.transform.position);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRot, targetSwitchSpeed);
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

        if (isLockedOn && possibleTargets.Count > 1)
        {
            isSwitching = true;

            if (Mathf.Abs(targetSwitchInput.x) > 0.35f) // Check for significant input
            {
                int change = targetSwitchInput.x > 0 ? 1 : -1;
                StartCoroutine(delaySwitching(change));
            }

            UpdateCombatCamera();
        }
        targetSwitchInput = Vector2.zero; // Clear input after processing

    }
    public IEnumerator delaySwitching(int change)
    {
        yield return new WaitForSecondsRealtime(.35f);
        yield return StartCoroutine(doTheSwitch(change));
    }

    public IEnumerator doTheSwitch(int change)
    {
        SwitchTargetIndex(change);
        yield return new WaitForSecondsRealtime(.15f);
    }

    public void SwitchTargetIndex(int change)
    {

        // Ensure we only switch if we are locked on and there are multiple targets
        if (isLockedOn && possibleTargets.Count > 1)
        {
            // Increment or decrement the currentTarget based on the change value
            if (change > 0)
            {
                currentTarget = (currentTarget + 1) % possibleTargets.Count; // Loop back to the start if at the end
            }
            else if (change < 0)
            {
                currentTarget = (currentTarget - 1 + possibleTargets.Count) % possibleTargets.Count; // Loop back to the end if at the start
            }

            // Update the lockOnTarget if it's different
            if (lockOnTarget != possibleTargets[currentTarget].lockOnPosition)
            {
                lockOnTarget = possibleTargets[currentTarget].lockOnPosition;
                //possibleTargets[currentTarget].CameraFollowsMe(playerCamera);
            }
        }
    }

    public void ToggleShoulder()
    {
        leftShoulder = !leftShoulder;
    }

    public void SwitchCameraShoulder()
    {
        if (isLockedOn && leftShoulder)
        {
            playerCamera.transform.position = Vector3.Slerp(lockedOnCameraPosition.position, lockedOnCameraPositionInverse.position, turnSpeed);

        }
        else
        {
            //leftShoulder = true;
            playerCamera.transform.position = Vector3.Slerp(lockedOnCameraPositionInverse.position, lockedOnCameraPosition.position, turnSpeed);
        }

    }

    public void ResetLockOn()
    {
        lockOnTarget = null;
        possibleTargets.Clear();
        currentTarget = 0;
    }
}
