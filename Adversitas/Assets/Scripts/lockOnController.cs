using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockOnController : MonoBehaviour
{
    [Header("-----LockOn-----")]
    public Transform lockOnTarget; // The target to lock onto
    private float lockOnDistance = 5f; // Distance from the target when locked on
    public bool isLockedOn = false; // Whether the camera is currently locked on
    private Transform playerPosition;
    private Camera playerCamera;
    private cameraSettings combatSettings;

    public void Initialize(Transform playerPosition, Camera playerCamera, float maxDistance, cameraSettings normal)
    {
        this.playerPosition = playerPosition;
        this.playerCamera = playerCamera;
        lockOnDistance = maxDistance;
        combatSettings = normal;
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
            ResetLockOn();
        }
    }

    public void UpdateCombatCamera(cameraSettings settings)
    {
        if (lockOnTarget != null)
        {
            playerCamera.transform.LookAt(lockOnTarget.position + Vector3.up * settings.height);
            combatSettings.SmoothCameraTransitions(combatSettings, settings);
        }
        else
        {
            playerCamera.transform.LookAt(playerPosition.position + Vector3.up * settings.height);
            combatSettings.SmoothCameraTransitions(combatSettings, settings);
        }
    }



    /// Performs based on foot position. 
    /// Needs to be given a solid center of mass position to help alleviate possible targeting errors
    /// Still functional
    public void FindCameraTarget()
    {
        Collider[] targets = Physics.OverlapSphere(playerPosition.position, lockOnDistance);

        if (targets.Length <= 0) return;

        if (isLockedOn && lockOnTarget != null)
        {
            //SwitchTarget();
            return;
        }

        Debug.Log("Searching for targets");
        for (int i = 0; i < targets.Length; i++)
        {
            GameObject parent = targets[i].gameObject;
            LockOnMe enemy;
            if (parent.TryGetComponent<LockOnMe>(out enemy))
            {
                if (IsTargetVisible(parent.transform))
                {
                    enemy.CameraFollowsMe(playerCamera);
                    lockOnTarget = enemy.lockOnPosition;
                    Debug.Log(enemy + " is the new target");
                    return;
                }
                else
                {
                    Debug.Log(enemy + " is obscured and cannot be targeted");
                }
            }
        }
        if (lockOnTarget == null)
        {
            UpdateCombatCamera(combatSettings);
            ToggleLockOn();
        }
    }

    private bool IsTargetVisible(Transform target)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerPosition.position, target.position, out hit, lockOnDistance))
        {
            if (hit.collider.gameObject.layer != 9)
            {
                Debug.Log("hit object was " + hit.collider.gameObject.name);
                return false;
            }
        }
        return true;
    }

    //public void SwitchTarget()
    //{

    //}

    public void ResetLockOn()
    {
        lockOnTarget = null;
    }
}
