using System;
using UnityEngine;

public class FollowTarget
{
    [SerializeField] public Transform myPosition;
    [SerializeField] public Transform followTargetsPosition;
    private float followDistance;
    private float distanceFromTarget;

    public bool iHaveATarget = false;

    public FollowTarget(Transform myPos, Transform toFollow = null, float distanceToFollowBy = 5)
	{
        followDistance = distanceToFollowBy;
        myPosition = myPos;
        if (toFollow != null)
        {
            followTargetsPosition = toFollow;
            iHaveATarget = true;
        }
        else
        {
            followTargetsPosition = myPosition;
            iHaveATarget = false;
        }

        RecalculateDistance();
    }

    public void RecalculateDistance()
    {
        if (followTargetsPosition == null || myPosition == null)
        {
            return;
        }
        distanceFromTarget = (followTargetsPosition.position - myPosition.position).magnitude;

        if (distanceFromTarget > followDistance)
        {
            Vector3 direction = (followTargetsPosition.position - myPosition.position).normalized;
            Vector3 newTargetPosition = myPosition.position + direction * followDistance;
            followTargetsPosition.position = newTargetPosition;
            float moveDistance = (newTargetPosition - myPosition.position).magnitude;
            MoveFollower(moveDistance, direction);
        }

        return distanceFromTarget;
    }

    public float GetDistanceFromTarget()
    {
        return distanceFromTarget;
    }

    public void AdjustDistance()
    {
        Vector3 directionToTarget = (followTargetsPosition.position - myPosition.position).normalized;
        float adjustedDistance = followDistance - distanceFromTarget;
        myPosition.position += directionToTarget * adjustedDistance;
    }

    private void MoveFollower(float moveDistance, Vector3 direction)
    {
        myPosition.position += direction * moveDistance;
    }

    public float GetDistanceFromTarget()
    {
        return distanceFromTarget;
    }

    public void SetFollowDistance(float newDistance)
    {
        followDistance = newDistance;
    }
}
