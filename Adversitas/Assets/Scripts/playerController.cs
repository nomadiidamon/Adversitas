using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class playerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 verticleVelocity;

    [Header("-----Ground Detection-----")]
    [SerializeField] BoxCollider leftFoot;
    [SerializeField] BoxCollider rightFoot;
    [SerializeField] float footRayLength;
    private FootRaycast leftFootRaycast;
    private FootRaycast rightFootRaycast;
    public bool onSolidSurface = false;
    public bool isJumping = false;
    public bool rotateWithCamera = false;
    public bool isIdle = false;


    [Header("-----Attributes -- Horizontal Speed-----")]
    [Range(0, 20)][SerializeField] float speed;
    [Range(0, 20)][SerializeField] float rotationSpeed;
    [Range(0, 5)][SerializeField] float sprintMod;
    public bool isSprinting = false;

    [Header("-----Attributes -- Verticle Speed-----")]
    [Range(0, 5)][SerializeField] int jumpMax;
    [Range(0, 20)][SerializeField] float jumpSpeed;
    [Range(0, 20)][SerializeField] float doubleJumpSpeed;
    [Range(0, 20)][SerializeField] float runningJumpSpeed;
    int jumpCount;

    [Header("-----Attributes -- Dodge Factors-----")]
    [Range(0, 5)][SerializeField] int dodgeMax;
    [Range(150, 300)][SerializeField] float dodgeSpeed;
    [Range(0, 20)][SerializeField] float dodgeDuration;
    int dodgeCount;
    public bool isDodging = false;

    [Header("-----Attributes -- State Timers-----")]
    [Range(0, 5)][SerializeField] float hardFallTime;
    [Range(0, 5)][SerializeField] float idleStartTime;
    private Stopwatch dodgeStopwatch;
    private Stopwatch fallStopwatch;
    private Stopwatch idleStopwatch;










    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
