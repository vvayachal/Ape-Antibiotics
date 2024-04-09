using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform shootPoint;
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
        if (Input.GetKeyUp(grappleKey))
        {
            StopGrapple();
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

        Debug.Log("Grapple");

        grappling = true;

        RaycastHit hit;
        Debug.DrawRay(shootPoint.position, shootPoint.forward, Color.red,1f);
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            Debug.Log($"We hit {hit.transform.name}");
            savedHit = hit;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = shootPoint.position + shootPoint.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
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
        positions[0] = shootPoint.position;
        positions[1] = hit.point;
        lr.SetPositions(positions);
    }
}
