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
    [SerializeField] private bool _isKnockbackable;

    [Tooltip("Controls how much knockback force is applied.")]
    [SerializeField] private float _knockbackForce;

    [Tooltip("Controls how much upward force is applied.")]
    [SerializeField] private float _knockbackUpwardsForce;

    // Manages state of knockback
    private bool _isKnocked = false;

    private void Awake()
    {
        // Search for the player
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isKnocked)
            EngageTarget();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Level") && _isKnocked)
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

        target.GetComponentInParent<Shield>().TakeDamage(10f);
        Debug.Log($"{this.name} has attacked {target.name}");
    }

    public void KnockBack(Vector3 knockbackOrigin)
    {
        if (_isKnockbackable && !_isKnocked)
        {
            Debug.Log("Knockback Applied");

            // Set the state
            _isKnocked = true;

            // Disable neccessary components
            animator.enabled = false;
            navMeshAgent.enabled = false;

            // Add "Knockback"
            rb.AddForce(Vector3.up * _knockbackForce, ForceMode.Impulse);
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
