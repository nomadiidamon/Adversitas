using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class playerController : MonoBehaviour, IMove, ILook, IJump
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator animator;

    [Header("-----Colliders-----")]
    [SerializeField] CapsuleCollider mainCollider;
    [SerializeField] CapsuleCollider damageCollider;
    [SerializeField] BoxCollider leftFoot;
    [SerializeField] BoxCollider rightFoot;

    [Header("-----Camera-----")]
    [SerializeField] Camera playerCamera;
    [Range(0, 5)][SerializeField] float cameraLookSpeed = 2f; // Speed of looking around
    [Range(0, 5)][SerializeField] float distanceFromPlayer = 5f; // Distance of camera from the player
    [Range(0, 5)][SerializeField] float height = 2f; // Height of the camera above the player
    [Range(0, 200)][SerializeField] float pitchLimit = 80f; // Limit for pitch to prevent flipping
    [Range(0, 5)][SerializeField] float playerTurnToCameraSpeed = 2f; // Speed for lerping toward camera direction


    [Header("-----Attributes-----")]
    [Range(0, 20)][SerializeField] float speed;
    [Range(0, 5)][SerializeField] float sprintMod;
    [Range(0, 5)][SerializeField] int jumpMax;
    [Range(0, 20)][SerializeField] float jumpSpeed;
    [Range(0, 20)][SerializeField] float doubleJumpSpeed;
    [Range(0, 30)][SerializeField] float gravity;

    public bool onSolidSurface = false;
    public bool isJumping = false;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 verticleVelocity;
    private float currentYaw;
    private float currentPitch;
    int jumpCount;


    void Awake()
    {
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions["Jump"].performed += ctx => OnJumpInput(ctx);


        animator.enabled = true;
        animator.stabilizeFeet = true;
    }

    void Update()
    {
        Look();
    }

    void FixedUpdate()
    {
        Move();
        isGrounded();
    }

    public void Move()
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        right.Normalize();

        // Combine movement input with camera's forward and right vectors
        Vector3 movement = (cameraForward * moveInput.y + right * moveInput.x).normalized;

        float currentSpeed = speed;
        transform.position += movement * currentSpeed * Time.deltaTime;

        animator.SetFloat("VelocityX", moveInput.x);
        animator.SetFloat("VelocityY", moveInput.y); 

        if (transform.rotation != playerCamera.transform.rotation)
        {
            Quaternion targetRotation = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerTurnToCameraSpeed);
        }
        Physics.SyncTransforms();
    }

    public void Look()
    {
        currentYaw += lookInput.x * cameraLookSpeed;
        currentPitch -= lookInput.y * cameraLookSpeed;

        currentPitch = Mathf.Clamp(currentPitch, -pitchLimit, pitchLimit);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer) + new Vector3(0, height, 0);
        playerCamera.transform.position = transform.position + offset;

        playerCamera.transform.LookAt(transform.position + Vector3.up * height);
        Physics.SyncTransforms();
    }

    public bool isGrounded()
    {

        if (rightFoot.GetComponent<FootRaycast>().isGrounded && leftFoot.GetComponent<FootRaycast>().isGrounded)
        {
            onSolidSurface = true;
            isJumping = false;
            jumpCount = 0;

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Falling Idle"))
            {
                animator.SetBool("IsGrounded", true);
            }
            return true;
        }
        else
        {
            onSolidSurface = false;
            animator.SetBool("IsGrounded", false);
            return false;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (isGrounded())
        {
            if(moveInput.x == 0 && moveInput.y == 0)
            {
                isJumping = true;
                animator.SetTrigger("SingleJump");
                jumpCount++;
            }
        }
        else 
        {
            if (!isGrounded() && jumpCount < jumpMax)
            {
                isJumping = true;
                animator.SetTrigger("SingleToDouble");
                jumpCount++;
            }
        }
        Physics.SyncTransforms();
    }

    public void ApplyJumpForce()
    {
        verticleVelocity.y = jumpSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void ApplySecondaryJumpForce()
    {
        verticleVelocity.y = doubleJumpSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void StartFalling()
    {
        verticleVelocity.y = 0;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

}
