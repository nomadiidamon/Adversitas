using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] public BoxCollider footCollider;

    [Header("-----Factors-----")]
    [SerializeField] public float rayLength = 0.5f;
    [SerializeField] public bool isGrounded = false; 

    void Update()
    {
        RaycastHit hit;
        Vector3 rayOrigin = footCollider.bounds.center;
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Environment") || hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                // Update jump animation state to grounded
                // E.g., animator.SetBool("isJumping", false);
            }
        }
        else
        {
            isGrounded = false;
            // Update jump animation state to jumping
            // E.g., animator.SetBool("isJumping", true);
        }

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, isGrounded ? Color.green : Color.red);

    }
}
