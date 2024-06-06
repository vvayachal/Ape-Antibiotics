using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    bool dashRequest;
    [HideInInspector] public bool canDash;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (pm.isGrounded)
        {
            canDash = true;
            
            if (Input.GetKeyDown(dashKey))
            {
                GameManager.Instance.cameraLines.Play();
                dashRequest = true;
            }

            if (dashCdTimer >0)
            {
                dashCdTimer -= Time.deltaTime;
            }
        }
        else
        {
            canDash = false;
        }
        
        
    }

    private void FixedUpdate()
    {
        if (dashRequest)
        {
            Dashing();
            dashRequest = false;
        }
    }

    private void Dashing()
    {
        if (dashCdTimer > 0)
        {
            return;
        }
        else
        {
            dashCdTimer = dashCd;
        }
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        // GameManager.Instance.cameraLines.Stop();
        Debug.Log("Dash Reset");
    }
}
