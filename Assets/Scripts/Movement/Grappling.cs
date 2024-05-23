using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    private DoubleJump dj;
    private Rigidbody rb;
    public Transform cam;
    public Transform shootPoint;
    public LayerMask whatIsGrappable;
    public LineRenderer lr;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject grappleGun;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;
    private SpringJoint joint;

    [SerializeField] float maxJointDistance = 0.8f;
    [SerializeField] float minJointDistance = 0.25f;
    [SerializeField] float jointSpringValue = 4.5f;
    [SerializeField] float jointDamperValue = 7f;
    [SerializeField] float jointMassScaleValue = 4.5f;


    //[Header("Cooldown")]
    //[Tooltip("The cooldown of the attack.")]
    //[SerializeField] float grapplingCd = 1f;
    //private WaitForSeconds grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.R;

    public bool grappling;
    private bool canGrapple = true;

    private RaycastHit savedHit;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        dj = GetComponent<DoubleJump>();
        rb = GetComponent<Rigidbody>();
        grappleGun.SetActive(false);

        //grapplingCdTimer = new WaitForSeconds(grapplingCd);
    }

    void Update()
    {
        if (Input.GetKeyDown(grappleKey) && canGrapple)
        {
            StartGrapple();
        }
        if (Input.GetKeyUp(grappleKey))
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        if (grappling)
        {
            setGrappleRope(savedHit);
        }
    }

    private void StartGrapple()
    {
        Debug.Log("Grapple");

        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, maxGrappleDistance, whatIsGrappable))
        {
            Debug.Log($"We hit {hit.transform.name}");
            savedHit = hit;
            ExecuteGrapplePhysics(savedHit);
        }
        else
        {
            return;
        }

        lr.enabled = true;
        grappleGun.SetActive(true);

        //StartCoroutine(GrappleCooldown());
    }

    //private IEnumerator GrappleCooldown()
    //{
    //    // Perform the cooldown
    //    yield return grapplingCdTimer;

    //    canGrapple = true;
    //}

    // change to fixed update
    private void ExecuteGrapplePhysics(RaycastHit hit)
    {
        Vector3 grapplePoint = hit.point;

        joint = pm.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;
        //joint.connectedBody = rb;

        float distanceFromPoint = Vector3.Distance(a: pm.gameObject.transform.position, b: grapplePoint);

        // The distance grapple will try to keep from grapple point. mess with this
        joint.maxDistance = distanceFromPoint * maxJointDistance;
        joint.minDistance = distanceFromPoint * minJointDistance;

        // Change these values until good fit
        joint.spring = jointSpringValue; // higher spring = more pull/push
        joint.damper = jointDamperValue;
        joint.massScale = jointMassScaleValue;

        // Let player double jump again
        dj.groundCheck.gameObject.SetActive(true);
    }

    private void StopGrapple()
    {
        grappling = false;
        grappleGun.SetActive(false);
        lr.enabled = false;
        Destroy(joint);
    }

    private void setGrappleRope(RaycastHit hit){
        Vector3[] positions = new Vector3[2];
        positions[0] = shootPoint.position;
        positions[1] = hit.point;
        lr.SetPositions(positions);
    }
}
