using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
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

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
    Animator animator;

    RaycastHit slopeHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        wr = GetComponent<WallRun>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jumpRequest = true;
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
            animator.SetTrigger("Jumping");
            jumpRequest = false;
        }

        ImproveJumping();

        DetectSpeed();
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Animator
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

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
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            speed = Mathf.Lerp(speed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, walkSpeed, acceleration * Time.deltaTime);
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
        var newValue = System.Math.Round(currentSpeed,1);

        speedText.text = newValue.ToString();
    }

    void ChangeYVelocity(float multiplier)
    {
        rb.velocity += ((multiplier - 1) * Physics.gravity.y * Vector3.up * Time.deltaTime);
    }
}
