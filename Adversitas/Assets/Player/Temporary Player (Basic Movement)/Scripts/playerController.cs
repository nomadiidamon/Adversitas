using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Unity.VisualScripting;

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
    [Range(0, 20)][SerializeField] float runningJumpSpeed;
    [Range(0, 5)][SerializeField] int dodgeMax;
    [Range(150, 300)][SerializeField] float dodgeSpeed;
    [Range(0, 20)][SerializeField] float dodgeDuration;
    [Range(0, 5)][SerializeField] float hardFallTime;

    private Stopwatch dodgeStopwatch;
    private Stopwatch fallStopwatch;


    private FootRaycast leftFootRaycast;
    private FootRaycast rightFootRaycast;
    public bool onSolidSurface = false;
    public bool isJumping = false;
    public bool isSprinting = false;
    public bool isDodging = false;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 verticleVelocity;
    private float currentYaw;
    private float currentPitch;
    int jumpCount;
    int dodgeCount;



    void Awake()
    {
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions["Jump"].performed += ctx => OnJumpInput(ctx);
        playerInput.actions["Sprint"].performed += ctx => { isSprinting = !isSprinting; };   
        playerInput.actions["Dodge"].performed += ctx => OnDodgeInput(ctx);  
        dodgeStopwatch = new Stopwatch();
        fallStopwatch = new Stopwatch();
        leftFootRaycast = leftFoot.GetComponent<FootRaycast>();
        rightFootRaycast = rightFoot.GetComponent<FootRaycast>();
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
        CheckDodgeTimer();
        CheckFallTimer();
    }

    /// Player movement logic
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

        if (isSprinting)
        {
            currentSpeed *= sprintMod;
            animator.SetTrigger("IsSprinting");

            if (moveInput.y == 0 && moveInput.x == 0)
            {
                isSprinting = false;
                animator.SetTrigger("IsWalking");
            }

        }
        else
        {
            currentSpeed /= sprintMod;
            animator.SetTrigger("IsWalking");
        }

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

    /// Camera look Logic - refer to CameraController for camera collision detection
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

    /// Check to see if player is on a solid surface - refer to FootRaycast for details
    public bool isGrounded()
    {

        if (rightFootRaycast.isGrounded && leftFootRaycast.isGrounded)
        {
            onSolidSurface = true;
            isJumping = false;
            jumpCount = 0;
            animator.SetFloat("FallTime", 0);
            animator.ResetTrigger("SingleToDouble");
            animator.ResetTrigger("SingleJump");

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Falling Idle"))
            {
                animator.SetBool("IsGrounded", true);
            }
            if (info.IsName("Hard Landing"))
            {
                animator.ResetTrigger("IsDodging");
            }
            return true;
        }
        else
        {
            fallStopwatch.Reset();
            fallStopwatch.Start();
            onSolidSurface = false;
            animator.SetBool("IsGrounded", false);
            return false;
        }
    }


    /// Jumping Logic
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

            if (moveInput.x == 0 && moveInput.y == 0)
            {
                isJumping = true;
                animator.SetTrigger("SingleJump");
                jumpCount++;
            }
            else
            {
                isJumping = true;
                animator.SetTrigger("MovingJump");
                jumpCount++;
            }

        }
        else 
        {
            if (!isGrounded() && jumpCount < jumpMax)
            {

                isJumping = true;
                animator.SetTrigger("SingleToDouble");
                //animator.SetFloat("FallTime", hardFallTime);
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

    public void ApplyHopForce()
    {
        verticleVelocity.y = jumpSpeed / 2;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void ApplySecondaryJumpForce()
    {
        verticleVelocity.y = doubleJumpSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void ApplyRunningJumpForce()
    {
        verticleVelocity.y = runningJumpSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void StartFalling()
    {
        verticleVelocity.y = 0;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = verticleVelocity;
    }

    public void CheckFallTimer()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Falling Idle"))
        {
            return;
        }

        if (!fallStopwatch.IsRunning)
        {
            fallStopwatch.Start();
        }

        if ((float)(fallStopwatch.ElapsedMilliseconds) > hardFallTime)
        {
            UnityEngine.Debug.Log((float)fallStopwatch.ElapsedMilliseconds);
            animator.SetFloat("FallTime", (float)fallStopwatch.ElapsedMilliseconds);
        }

    }




    /// Dodging Logic
    public void OnDodgeInput(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            Dodge();
        }
    }

    public void Dodge()
    {
        if (isDodging)
        {
            return;
        }
        else
        {
            if (isGrounded())
            {
                dodgeStopwatch.Reset();
                animator.SetFloat("DodgeTime", dodgeDuration);
                dodgeStopwatch.Start();
                isDodging = true;
                animator.SetTrigger("IsDodging");
                dodgeCount++;
            }
            else
            {
                if (dodgeCount < dodgeMax)
                {
                    dodgeStopwatch.Reset();
                    animator.SetFloat("DodgeTime", dodgeDuration);
                    dodgeStopwatch.Start();
                    isDodging = true;
                    animator.SetTrigger("IsDodging");
                    dodgeCount++;
                }
            }
            
        }
    }

    public void CheckDodgeTimer()
    {
        if (isDodging && (float)(dodgeStopwatch.ElapsedMilliseconds) > dodgeDuration)
        {
            animator.SetFloat("DodgeTime", 0);
            dodgeStopwatch.Stop();

            //possibly unecessary due to funciton being a animation event
            //
            RemoveDodgeForce();
            //
        }
    }


    public void ApplyDodgeForce()
    {
        // Get the forward and right vectors relative to the camera
        Vector3 dodgeDirection = (playerCamera.transform.forward * moveInput.y + playerCamera.transform.right * moveInput.x).normalized;

        // If no input, dodge forward
        if (dodgeDirection == Vector3.zero)
            dodgeDirection = transform.forward;

        // Add force only in the horizontal direction, ignoring any vertical movement
        dodgeDirection.y = 0;

        Rigidbody rb = GetComponent<Rigidbody>();

        // Apply the dodge force as an impulse for a burst of speed
        rb.AddForce(dodgeDirection * (dodgeSpeed), ForceMode.Impulse);
        rb.AddForce(dodgeDirection * (dodgeSpeed), ForceMode.Impulse);

    }


    public void RemoveDodgeForce()
    {
        // Reset the horizontal velocity (keep the vertical velocity intact if the player is in the air)
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        isDodging = false;
        dodgeCount = 0;

    }
}
