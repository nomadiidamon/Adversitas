using Cinemachine;
using System;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
public class MoveToDestinationRigidbody : AIComponent, IMove, IAIComponent
{   
    private Rigidbody objectRigidbody;
    [SerializeField] public Transform destinationTransform;
    [SerializeField] public Vector3 destinationVector3;
    [Range(0, 290)][SerializeField] public float moveSpeed;
    [Range(2.9f, 50)][SerializeField] public float stoppingDistance;
    [SerializeField] public bool isAtDestination = false;

    //public MoveToDestinationRigidbody(Rigidbody objectRigidbody, Transform destination, float moveSpeed)
    //{
    //    this.objectRigidbody = objectRigidbody;
    //    this.destinationTransform = destination;
    //    this.destinationVector3 = destinationTransform.position;
    //    this.moveSpeed = moveSpeed;
    //}

    public void Start()
    {
        if (destinationTransform != null)
        {
            SetDestination(destinationTransform);
            destinationVector3 = destinationTransform.localPosition;
            objectRigidbody = GetComponentInParent<Rigidbody>();
        }
    }

    public void FixedUpdate()
    {
        PerformRole();
    }

    public override void PerformRole()
    {
        if (isAtDestination)
        {
            return;
        }
        Move();
    }

    public void Move()
    {
        if (objectRigidbody == null) return;

        if ((destinationVector3 - objectRigidbody.position).magnitude < stoppingDistance)
        {
            isAtDestination = true;
            return;
        }



        Vector3 direction = (destinationVector3 - objectRigidbody.position).normalized;
        direction.y = 0;
        //Debug.Log("Direction: " + direction);
        objectRigidbody.transform.rotation.SetLookRotation(direction);
        objectRigidbody.MovePosition(objectRigidbody.position + direction * moveSpeed * Time.deltaTime);
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;

        //Debug.Log("Speed changed to: " + moveSpeed);
    }

    public float GetMoveSpeed()
    {
        //.Log("Speed is: " + moveSpeed);
        return moveSpeed;
    }

    public void SetDestination(Vector3 newDestination)
    {
        if (newDestination != destinationVector3)
        {
            isAtDestination = false;
            destinationVector3 = newDestination;
            //Debug.Log("Destination changed to: " + destinationVector3);

        }
    }

    public void SetDestination(Transform newDestination)
    {
        destinationTransform = newDestination;
        SetDestination(newDestination.position);
    }


    public Vector3 GetDestinationVector3()
    {
        //Debug.Log("Destination is: " + destinationVector3);
        return GetDestinationTransform().position;
    }

    public Transform GetDestinationTransform()
    {
        //Debug.Log("Destination is: " + destinationTransform);
        return destinationTransform;
    }

    public void SetStoppingDistance(float newStoppingDistance)
    {
        stoppingDistance = newStoppingDistance;
    }

    private void OnValidate()
    {
        SetDestination(GetDestinationTransform());
    }

}