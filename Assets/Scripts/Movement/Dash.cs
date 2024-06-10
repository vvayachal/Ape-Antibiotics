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

    [Header("Cooldown")]
    [Tooltip("The cooldown time of this ability.")]
    public float DashCd;

    [SerializeField] [Tooltip("DEBUG - TIME LEFT")]
    private float dashCdTimerLeft;

    [Tooltip("Reference to the CooldownManager.")]
    public CooldownManager cooldownManager; // Reference to the CooldownManager
    
    [Tooltip("Index of the cooldown icon in CooldownManager.")]
    public int cooldownIconIndex = 1; // Index of the cooldown icon in CooldownManager


    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            dashRequest = true;
        }

        if (dashCdTimerLeft > 0)
        {
            dashCdTimerLeft -= Time.deltaTime;
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
        if (dashCdTimerLeft > 0)
        {
            return;
        }
        else
        {
            dashCdTimerLeft = DashCd;
            if (cooldownManager != null)
            {
                cooldownManager.TriggerCooldown(cooldownIconIndex,DashCd); // Start the cooldown animation
            }
        }

        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        Debug.Log("Dash Reset");
    }
}
