using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraCollisionController : MonoBehaviour
{
    /// <summary>
    /// Must be attached to the player's camera to work properly
    /// </summary>


    [Header("-----Components-----")]
    [SerializeField] SphereCollider cameraCollider;
    [SerializeField] Rigidbody rb;

    [Header("-----Attributes-----")]
    [SerializeField] float maxDistance;     // Maximum distance from the target
    [SerializeField] float collisionSmoothSpeed;    // speed at which the camera changes positions
    [SerializeField] float stoppingDistance;    // distance at which the camera stops from the player when adjusting for collisions
    [SerializeField] LayerMask collisionLayerMask;  // layers to include when adjusting for collisions
    [SerializeField] float minCollisionSmoothSpeed = 7f;

    private Vector3 originalLocalPosition;
    private Transform cameraPosition;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        originalLocalPosition = transform.localPosition;
        cameraPosition = transform.parent;

        rb.isKinematic = true;

        PhysicMaterial physicsMaterial = new PhysicMaterial
        {
            frictionCombine = PhysicMaterialCombine.Average,
            bounciness = 0f,
            dynamicFriction = 0f,
            staticFriction = 0f,
        };

        cameraCollider.sharedMaterial = physicsMaterial;

        // minimum for best found collision smoothing
        if (collisionSmoothSpeed < minCollisionSmoothSpeed)
            collisionSmoothSpeed = minCollisionSmoothSpeed;
    }

    //void Update()
    //{
    //    HandleCameraCollision();
    //}

    private void FixedUpdate()
    {
        HandleCameraCollision();
    }

    private void HandleCameraCollision()
    {
        // Step 1: Calculate Desired Position
        Vector3 desiredPosition = CalculateDesiredPosition();
        Debug.DrawLine(cameraPosition.position, desiredPosition, Color.green);

        // Step 2: Perform SphereCast and Check Collision
        RaycastHit hit;
        bool isColliding = PerformSphereCast(desiredPosition, out hit);
        DebugSphereCastVisualization(cameraPosition.position, desiredPosition, isColliding, hit);

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
        return cameraPosition.TransformPoint(originalLocalPosition);
    }

    private bool PerformSphereCast(Vector3 desiredPosition, out RaycastHit hit)
    {
        // Calculate direction and distance for SphereCast
        Vector3 directionToDesired = (desiredPosition - cameraPosition.position).normalized;
        float distanceToDesired = Vector3.Distance(cameraPosition.position, desiredPosition);

        // Perform the SphereCast
        return Physics.SphereCast(cameraPosition.position, cameraCollider.radius, directionToDesired, out hit, distanceToDesired, collisionLayerMask);
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
        Vector3 collisionPosition = cameraPosition.InverseTransformPoint(hit.point) + (cameraPosition.InverseTransformDirection(hit.normal) * (cameraCollider.radius + stoppingDistance));
        transform.localPosition = Vector3.Lerp(transform.localPosition, collisionPosition, Time.deltaTime * collisionSmoothSpeed);
    }

    private void ReturnToOriginalPosition()
    {
        // Calculate the safe position to maintain the stopping distance
        Vector3 safePosition = originalLocalPosition;
        float distanceToPlayer = Vector3.Distance(cameraPosition.position, cameraPosition.TransformPoint(originalLocalPosition));

        if (distanceToPlayer < stoppingDistance)
        {
            // Adjust the position to maintain the stopping distance from the player
            Vector3 directionToCamera = (cameraPosition.position - cameraPosition.TransformPoint(originalLocalPosition)).normalized;
            safePosition = originalLocalPosition + (cameraPosition.InverseTransformDirection(directionToCamera) * stoppingDistance);
        }

        // Return the camera to its original position, considering stopping distance
        transform.localPosition = Vector3.Lerp(transform.localPosition, safePosition, Time.deltaTime * collisionSmoothSpeed);
    }


}
