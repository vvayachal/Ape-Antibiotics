using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : MonoBehaviour
{
    //to those who take a look at this, I'm trying to make it feel better :)
    //possible I may need to redo it entirely

    //aoe damage 
    public Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    private Knockback knockb;
    public Transform smashPoint;
    public float shockRange = 0.5f;
    public LayerMask enemyLayers;

    float groundDistance = 0.4f;

    public float descendRate = 20f;
    public float damage = 10f;
    public Transform groundPoundPoint;
    public float attackRange = 2f;
    public float cooldown;

    bool isGrounded;
    bool groundPoundRequest;

    PlayerMovement pm;
    Rigidbody rb;

    //make the ground pound similar to ultrakill

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        groundCheck.gameObject.SetActive(false);
    }

    void Update()
    {
        DetectJump();
        DetectGroundPound();
    }

    private void FixedUpdate()
    {
        if (groundPoundRequest)
        {
            if(!isGrounded)
            {
                PerformGroundPound();
                groundPoundRequest = false;
                pm.isJumping = false;
            }
        }
    }

    private void DetectJump()
    {
        // Activate secondary groundcheck after initial jump
        if (pm.isJumping)
        {
            groundCheck.gameObject.SetActive(true);
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
    }

    void DetectGroundPound()
    {
        if (groundCheck.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded)
            {
                Debug.Log("Ground Pound!");
                groundPoundRequest = true;
            }
        }
    }

    void PerformGroundPound()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * -(descendRate), ForceMode.Impulse);
        groundCheck.gameObject.SetActive(false);
        StartGroundPound();
    }

    void StartGroundPound()
    { 
        Debug.Log("Attacking!");

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(groundPoundPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach (Collider enemy in hitEnemies)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this
            Debug.Log($"{this.name} has ground pounded {enemy.name}");
            if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
            {
                enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        
            // Knock them back
            StartCoroutine(knockb.ApplyKnockBack(enemy, -(groundPoundPoint.forward), damage));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundPoundPoint == null) return;

        if (groundPoundPoint.parent.gameObject.activeSelf)
        {
            Gizmos.DrawWireSphere(groundPoundPoint.position, attackRange);
        }
    }
}


