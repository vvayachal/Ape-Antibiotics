using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;

    [Header("Detection")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    bool wallRunRequest;
    bool jumpRequest;
    bool stopRunRequest;
    public bool wallRunning;
    public float wallRunDrag;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt { get; private set; }

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                wallRunRequest = true;
                ChangeCameraView(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumpRequest = true;
                }
            }
            else if (wallRight)
            {
                wallRunRequest = true;
                ChangeCameraView(false);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumpRequest = true;
                }
            }
            else
            {
                stopRunRequest = true;
                NormalizeCameraView();
            }
        }
        else
        {
            stopRunRequest = true;
            NormalizeCameraView();
        }
    }

    private void FixedUpdate()
    {
        if (wallRunRequest)
        {
            GameManager.Instance.cameraLines.Play();
            StartWallRun();

            wallRunRequest = false;
        }

        if (jumpRequest)
        {
            JumpOffWall();
            
            jumpRequest = false;
        }

        if (stopRunRequest)
        {
            GameManager.Instance.cameraLines.Stop();
            StopWallRun();

            stopRunRequest = false;
        }
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    //Essentially checks if player has jumped high enough off the ground to wall run
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void ChangeCameraView(bool wallDirection)
    {
        if (wallRunRequest)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

            if (wallDirection)
            {
                tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            }
            else
            {
                tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            }
        }
    }

    void NormalizeCameraView()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
    }

    void JumpOffWall()
    {
        if (wallLeft)
        {
            Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
        }
        else if (wallRight)
        {
            Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;
    }
}
