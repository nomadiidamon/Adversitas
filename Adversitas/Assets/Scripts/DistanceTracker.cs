using System;
using UnityEngine;

public class DistanceTracker
{
	[SerializeField] public Transform myPosition;
	[SerializeField] public Transform targetPosition;
	private float distanceFromTarget;
	public bool iHaveATarget = false;


	public DistanceTracker(Transform myPos, Transform target = null)
	{
		myPosition = myPos;
		if (target != null)
		{
			targetPosition = target;
			iHaveATarget = true;
		}
		else
		{
			targetPosition = myPosition;
			iHaveATarget = false;
		}
		RecalculateDistance();
	}

	public float RecalculateDistance()
	{
		if (targetPosition == myPosition)
		{
			return 333.333f;
		}
        distanceFromTarget = (targetPosition.position - myPosition.position).magnitude;
		return distanceFromTarget;
    }

	public float GetDistanceFromTarget()
	{
		return distanceFromTarget;
	}
}
