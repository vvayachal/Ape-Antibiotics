using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyMotor : MonoBehaviour, IKnockable
{
    Transform target; // Changed target to search for the player in awake [Tegomlee]
    NavMeshAgent navMeshAgent;
    Animator animator;
    Rigidbody rb;

    //-----------------------

    [Header("NavMesh Attributes")]

    [SerializeField] float chaseRange = 5f; // <- This isn't used in the code except for the OnDrawGizmos(), Would remove [Tegomlee]
    float distanceToTarget = Mathf.Infinity; // <- I changed the NavMesh Stopping Distance to 0. This is a wierd line [Tegomlee]

    //-----------------------

    [Header("KnockBack Attributes")]

    [Tooltip("Controls wether the enemy is able to be knocked back. (Mainly for debugging purposes).")]
    [SerializeField] bool _isKnockbackable;

    [Tooltip("Controls how much knockback force is applied to this specific enemy (Lower is lighter)."), Range(0.2f, 6f)]
    [SerializeField] float _knockbackForceMultplier = 1f;

    [Tooltip("Layer(s) the enemy considers as walkable.")]
    [SerializeField] LayerMask _walkableLayer;

    //----------

    // Manages state of knockback
    private bool _isKnocked = false;

    private void Awake()
    {
        // Search for the player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Assign component references
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isKnocked)
            EngageTarget();
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMaskExtensions.IsLayerInMask(_walkableLayer, collision.gameObject.layer))
        {
            Recover();
        }
    }

    void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetBool("attack", false);
        }
    }

    void AttackTarget()
    {
        animator.SetBool("attack", true);
    }

    void AttackHitEvent()
    {
        if (target == null) { return; }

        target.GetComponentInParent<PlayerShield>().TakeDamage(10f);
        Debug.Log($"{this.name} has attacked {target.name}");
    }

    public void KnockBack(Vector3 knockbackOrigin, float baseKnockbackForce)
    {
        if (_isKnockbackable && !_isKnocked)
        {
            Debug.Log("Knockback Applied");

            // Calculate the direction of the knockback
            Vector3 knockbackDirection = transform.position - knockbackOrigin;

            // Normalize the direction to eliminate the magnitude
            knockbackDirection.Normalize();

            // Set the state
            _isKnocked = true;

            // Disable neccessary components
            animator.enabled = false;
            navMeshAgent.enabled = false;

            // Add "Knockback"
            // I use ForceMode.VelocityChange becuase the designer will modify each enemies knockback force multiplier directly in this script's inspector.
            rb.AddForce(knockbackDirection * baseKnockbackForce * _knockbackForceMultplier, ForceMode.VelocityChange);
        }
    }

    public void Recover()
    {
        if (_isKnocked)
        {
            // Reset the state
            _isKnocked = false;

            // Enable neccesary components
            animator.enabled = true;
            navMeshAgent.enabled = true;
        }
    }

    // Controls the wire mesh that determines how close player
    // must be for enemy to chase player
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
