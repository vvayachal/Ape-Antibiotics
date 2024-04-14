using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //camera
    [SerializeField] private Transform cameraTarget;

    //mouse look/move
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private bool InvertMouse = false;
    [SerializeField] private float LookUpConstraint = 60f;
    [SerializeField] private float LookDownConstraint = -60f;

    //movement
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 15f;
    
    //jumping    
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityMod = 2.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;


    //local GameObjects
    private CharacterController charController;
    private Camera mainCamera;


    //local variables
    private float verticalRotationStore;
    private Vector3 movement;
    private bool isGrounded;


    void Start()
    {
        charController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        //mouse input
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        //rotate character based on x mouse movement
        float yRotation = transform.rotation.eulerAngles.y + mouseInput.x;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);


        //mouse invert preference multiplier
        float invert = (!InvertMouse) ? -1f : 1f;
        verticalRotationStore += (mouseInput.y * invert);

        //limit X rotation value based on input
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, LookDownConstraint, LookUpConstraint);

        //apply rotation to camera target
        cameraTarget.rotation = Quaternion.Euler(verticalRotationStore, cameraTarget.eulerAngles.y, cameraTarget.eulerAngles.z);

        //get user input for moving
        Vector3 moveForward = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 moveRight = transform.right * Input.GetAxisRaw("Horizontal");

        //set movement speed based on if the player is holding down left shift
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : moveSpeed;

        //jump check
        float yVelocity = movement.y;
        movement = (moveForward + moveRight).normalized;
        movement *= currentSpeed;
        movement.y = yVelocity;

        //check if Character Controller is grounded
        if (charController.isGrounded)
        {
            movement.y = 0f;
        }

        //do our own check for jumping
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.25f, groundLayers);
        
        //if player is pushing the jump button
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }

        //apply gravity to the player
        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        //apply movement to the player
        charController.Move(movement * Time.deltaTime);

    }

    private void LateUpdate()
    {
        //change main camera position and rotation based on cameraTarget
        mainCamera.transform.position = cameraTarget.position;
        mainCamera.transform.rotation = cameraTarget.rotation;
    }
}
