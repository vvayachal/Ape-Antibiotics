using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IKnockable
{
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && isGrounded && !wr.wallRunning;
    private bool _isKnocked = false;
    
    [Tooltip("Controls whether the player is to be knocked back. (Mainly for debugging purposes).")]
    [SerializeField] private bool _isKnockbackable;

    [Tooltip("Controls the knockback force."), Range(0.2f, 6f)]
    [SerializeField] private float _knockbackForceMultplier;
    
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    
    [Header("Movement")]
    float speed;
    [SerializeField] Text speedText;
    private float currentSpeed;
    WallRun wr;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    bool jumpRequest;
    public bool isJumping;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;
    public bool isGrounded;

    [Header("Crouch Parameters")]
    [SerializeField] private float timeToCrouch;
    [SerializeField] private float crouchingHeight;
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchingMoveSpeed;
    private CapsuleCollider _capsuleCollider;
    [SerializeField] private bool isCrouching;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        wr = GetComponent<WallRun>();
        _capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jumpRequest = true;
        }

        if (ShouldCrouch)
        {
            HandleCrouch();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ControlSpeed();
        ControlDrag();

        if (jumpRequest)
        {
            Jump();

            jumpRequest = false;
        }

        ImproveJumping();

        DetectSpeed();
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = (orientation.forward * verticalMovement) + (orientation.right * horizontalMovement);
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * speed, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Acceleration);
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void ControlSpeed()
    {
        if (!isCrouching && Input.GetKey(sprintKey) && isGrounded)
        {
            speed = Mathf.Lerp(speed, sprintSpeed, acceleration * Time.deltaTime);
            GameManager.Instance.cameraLines.Play();
        }
        else
        {
            if(!isCrouching)
            {
                speed = Mathf.Lerp(speed, walkSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                speed = Mathf.Lerp(speed, crouchingMoveSpeed, acceleration * Time.deltaTime);
            }
            GameManager.Instance.cameraLines.Stop();
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (wr.wallRunning)
        {
            rb.drag = wr.wallRunDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            isJumping = true;
        }
    }

    void ImproveJumping()
    {
        if (rb.velocity.y < 0)
        {
            ChangeYVelocity(fallMultiplier);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            ChangeYVelocity(lowJumpMultiplier);
        }
    }

    void DetectSpeed()
    {
        currentSpeed = ((float)rb.velocity.magnitude);
        // Round to nearest tenth decimal
        var newValue = System.Math.Round(currentSpeed, 1);

        if(speedText)
        {
            speedText.text = newValue.ToString();
        }
        
    }

    void ChangeYVelocity(float multiplier)
    {
        rb.velocity += ((multiplier - 1) * Physics.gravity.y * Vector3.up * Time.deltaTime);
    }

    void HandleCrouch()
    {
        StartCoroutine(Crouch());
    }

    private IEnumerator Crouch()
    {
        float timeElapsed = 0f;
        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
        float currentHeight = _capsuleCollider.height;

        while (timeElapsed < timeToCrouch)
        {
            _capsuleCollider.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _capsuleCollider.height = targetHeight;

        isCrouching = !isCrouching;
    }

    public void KnockBack(Vector3 knockbackOrigin, float baseKnockbackForce)
    {
        if (_isKnockbackable && !_isKnocked)
        {
            Debug.Log("Knockback Applied");

            // Calculate the direction of the knockback
            Vector3 knockbackDirection = transform.position - knockbackOrigin;

            // Normalize the direction to eliminate the magnitude
            knockbackDirection.Normalize();

            // Set the state
            _isKnocked = true;

            // Disable neccessary components

            if (TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = false;    
            }

            if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.enabled = false;
            }

            // Add "Knockback"
            // I use ForceMode.VelocityChange becuase the designer will modify each enemies knockback force multiplier directly in this script's inspector.
            rb.AddForce(knockbackDirection * baseKnockbackForce * _knockbackForceMultplier, ForceMode.VelocityChange);
            
            Recover();
        }
    }
    
    public void Recover()
    {
        if (_isKnocked)
        {
            // Reset the state
            _isKnocked = false;

            // Enable neccesary components
            if (TryGetComponent<Animator>(out Animator animator))
            {
                animator.enabled = true;    
            }
            
            if (TryGetComponent<NavMeshAgent>(out NavMeshAgent navMeshAgent))
                navMeshAgent.enabled = true;
        }
    }
}
