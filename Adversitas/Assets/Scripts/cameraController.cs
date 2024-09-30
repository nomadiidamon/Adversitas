using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] SphereCollider cameraCollider;    // Radius of the sphere collider
    [SerializeField] Rigidbody rb;

    [Header("-----Attributes-----")]
    [SerializeField] float maxDistance;     // Maximum distance from the target
    [SerializeField] float collisionSmoothSpeed;
    [SerializeField] float stoppingDistance;
    [SerializeField] LayerMask collisionLayerMask;

    private Vector3 originalLocalPosition;




    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Store the camera's original local position
        originalLocalPosition = transform.localPosition;

        // Add a Rigidbody to the camera and set it to Kinematic
        rb.isKinematic = true;

        // Optional: Assign a Physics Material with zero friction and bounciness
        PhysicMaterial physicsMaterial = new PhysicMaterial
        {
            frictionCombine = PhysicMaterialCombine.Average,
            bounciness = 0f,
            dynamicFriction = 0f,
            staticFriction = 0f,
        };

        cameraCollider.sharedMaterial = physicsMaterial;

        // Check to ensure Collision Smooth Speed is greater than 7 to prevent viewing through walls
        if (collisionSmoothSpeed < 7)
            collisionSmoothSpeed = 7;
    }

    void Update()
    {
        HandleCameraCollision();
    }

    private void HandleCameraCollision()
    {
        // Step 1: Calculate Desired Position
        Vector3 desiredPosition = CalculateDesiredPosition();
        Debug.DrawLine(transform.parent.position, desiredPosition, Color.green);

        // Step 2: Perform SphereCast and Check Collision
        RaycastHit hit;
        bool isColliding = PerformSphereCast(desiredPosition, out hit);
        DebugSphereCastVisualization(transform.parent.position, desiredPosition, isColliding, hit);

        // Step 3: Handle Collision Response
        if (isColliding)
        {
            HandleCollision(hit);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private Vector3 CalculateDesiredPosition()
    {
        // Calculate the desired position based on the original local position
        return transform.parent.TransformPoint(originalLocalPosition);
    }

    private bool PerformSphereCast(Vector3 desiredPosition, out RaycastHit hit)
    {
        // Calculate direction and distance for SphereCast
        Vector3 directionToDesired = (desiredPosition - transform.parent.position).normalized;
        float distanceToDesired = Vector3.Distance(transform.parent.position, desiredPosition);

        // Perform the SphereCast
        return Physics.SphereCast(transform.parent.position, cameraCollider.radius, directionToDesired, out hit, distanceToDesired, collisionLayerMask);
    }

    private void DebugSphereCastVisualization(Vector3 startPosition, Vector3 desiredPosition, bool isColliding, RaycastHit hit)
    {
        // Draw SphereCast direction and hit visualization
        Debug.DrawRay(startPosition, (desiredPosition - startPosition).normalized * Vector3.Distance(startPosition, desiredPosition), Color.red);
        if (isColliding)
        {
            Debug.DrawRay(hit.point, hit.normal * 2, Color.yellow); // Draw the hit normal to visualize the collision point
        }
    }

    private void HandleCollision(RaycastHit hit)
    {
        // Calculate the collision position to avoid the obstacle
        Vector3 collisionPosition = transform.parent.InverseTransformPoint(hit.point) + (transform.parent.InverseTransformDirection(hit.normal) * (cameraCollider.radius + stoppingDistance));
        transform.localPosition = Vector3.Lerp(transform.localPosition, collisionPosition, Time.deltaTime * collisionSmoothSpeed);
    }

    private void ReturnToOriginalPosition()
    {
        // Calculate the safe position to maintain the stopping distance
        Vector3 safePosition = originalLocalPosition;
        float distanceToPlayer = Vector3.Distance(transform.parent.position, transform.parent.TransformPoint(originalLocalPosition));

        if (distanceToPlayer < stoppingDistance)
        {
            // Adjust the position to maintain the stopping distance from the player
            Vector3 directionToCamera = (transform.parent.position - transform.parent.TransformPoint(originalLocalPosition)).normalized;
            safePosition = originalLocalPosition + (transform.parent.InverseTransformDirection(directionToCamera) * stoppingDistance);
        }

        // Return the camera to its original position, considering stopping distance
        transform.localPosition = Vector3.Lerp(transform.localPosition, safePosition, Time.deltaTime * collisionSmoothSpeed);
    }


}
