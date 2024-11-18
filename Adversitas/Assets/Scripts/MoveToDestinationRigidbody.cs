using UnityEngine;

public class MoveToDestinationRigidbody : IMove
{
    private Rigidbody objectRigidbody;
    private Vector3 destination;
    private float moveSpeed;

    public MoveToDestinationRigidbody(Rigidbody objectRigidbody, Vector3 destination, float moveSpeed)
    {
        this.objectRigidbody = objectRigidbody;
        this.destination = destination;
        this.moveSpeed = moveSpeed;
    }

    public void Move()
    {
        if (objectRigidbody == null) return;

        Vector3 direction = (destination - objectRigidbody.position).normalized;

        objectRigidbody.MovePosition(objectRigidbody.position + direction * moveSpeed * Time.deltaTime);
    }
}