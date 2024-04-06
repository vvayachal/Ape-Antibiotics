using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappable;
    public LineRenderer lr;
    [SerializeField] GameObject cameraHolder;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.R;

    private bool grappling;

    private RaycastHit savedHit;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }

        if(grappling){
            setGrappleRope(savedHit);
        }
        //lr.gameObject.transform.localRotation = new Quaternion(cameraHolder.transform.localRotation.x, lr.gameObject.transform.localRotation.y, lr.gameObject.transform.localRotation.z, lr.gameObject.transform.localRotation.w);
    }

    private void StartGrapple()
    {
        //if (grapplingCdTimer > 0)
        //{
        //    return;
        //}

        grappling = true;

        RaycastHit hit;
        Debug.DrawRay(gunTip.position, gunTip.forward, Color.red,1f);
        if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            Debug.Log($"We hit {hit.transform.name}");
            savedHit = hit;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = gunTip.position + gunTip.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        // lr.SetPosition(1, grapplePoint);
        setGrappleRope(hit);

    }

    private void ExecuteGrapple()
    {

    }

    private void StopGrapple()
    {
        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }

    private void setGrappleRope(RaycastHit hit){
        Vector3[] positions = new Vector3[2];
        positions[0] = gunTip.position;
        positions[1] = hit.point;
        lr.SetPositions(positions);
    }
}
