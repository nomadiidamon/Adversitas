using Cinemachine;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
public class MoveToDestination : IMove, IAIComponent
{
    private Transform objectTransform;
    private Vector3 destination;
    private float moveSpeed;


    public MoveToDestination(Transform objectTransform, Vector3 destination, float moveSpeed)
    {
        this.objectTransform = objectTransform;
        this.destination = destination;
        this.moveSpeed = moveSpeed;
    }

    public void PerformRole()
    {
        Move();
    }

    public void Move()
    {
        if (objectTransform == null) return;

        objectTransform.position = Vector3.MoveTowards(objectTransform.position, destination, moveSpeed * Time.deltaTime);
    }
}
