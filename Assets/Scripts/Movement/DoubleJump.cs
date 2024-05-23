using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    bool jumpRequest;
    public bool isGrounded;

    PlayerMovement pm;
    Rigidbody rb;
    Grappling grapple;
    Animator animator;

    float groundDistance = 0.4f;
    float jumpForce; // Same jump force as jump force from PlayerMovement script - azicii

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        jumpForce = GetComponent<PlayerMovement>().jumpForce;
        animator = GetComponentInChildren<Animator>();

        groundCheck.gameObject.SetActive(false);
    }

    void Update()
    {
        DetectJump();
        DetectSecondJump();
    }

    void DetectJump()
    {
        // Activate secondary groundcheck after initial jump
        if (pm.isJumping)
        {
            groundCheck.gameObject.SetActive(true);
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }

    void DetectSecondJump()
    {
        if (groundCheck.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(jumpKey) && !isGrounded)
            {
                jumpRequest = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            PerformDoubleJump();

            jumpRequest = false;
            pm.isJumping = false;
        }
    }

    void PerformDoubleJump()
    {
        animator.SetTrigger("Jumping");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        groundCheck.gameObject.SetActive(false);
    }
}
