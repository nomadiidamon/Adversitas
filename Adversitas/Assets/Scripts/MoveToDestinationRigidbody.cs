using Cinemachine;
using System;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
[RequireComponent (typeof(Rigidbody))]
public class MoveToDestinationRigidbody : MonoBehaviour, IMove, IAIComponent
{   private Rigidbody objectRigidbody;
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private Vector3 destinationVector3;
    [SerializeField] private float moveSpeed;

    public MoveToDestinationRigidbody(Rigidbody objectRigidbody, Transform destination, float moveSpeed)
    {
        this.objectRigidbody = objectRigidbody;
        this.destinationTransform = destination;
        this.destinationVector3 = destinationTransform.position;
        this.moveSpeed = moveSpeed;
    }

    public void PerformRole()
    {
        Move();
    }

    public void Move()
    {
        if (objectRigidbody == null) return;

        Vector3 direction = (destinationVector3 - objectRigidbody.position).normalized;

        objectRigidbody.MovePosition(objectRigidbody.position + direction * moveSpeed * Time.deltaTime);
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;

        Debug.Log("Speed changed to: " + moveSpeed);
    }

    public float GetMoveSpeed()
    {
        Debug.Log("Speed is: " + moveSpeed);
        return moveSpeed;
    }

    public void SetDestination(Vector3 newDestination)
    {
        destinationVector3 = newDestination;

        Debug.Log("Destination changed to: " + destinationVector3);
    }

    public void SetDestination(Transform newDestination)
    {
        SetDestination(newDestination.position);
    }


    public Vector3 GetDestinationVector3()
    {
        Debug.Log("Destination is: " + destinationVector3);
        return GetDestinationTransform().position;
    }

    public Transform GetDestinationTransform()
    {
        Debug.Log("Destination is: " + destinationTransform);
        return destinationTransform;
    }



}