using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    [Header("NavMesh Attributes")]

    [SerializeField] float chaseRange = 5f; // <- This isn't used in the code except for the OnDrawGizmos(), Would remove [Tegomlee]
    float distanceToTarget = Mathf.Infinity; // <- I changed the NavMesh Stopping Distance to 0. This is a wierd line [Tegomlee]

    //-----------------------

    [Header("KnockBack Attributes")]

    [Tooltip("Controls wether the enemy is able to be knocked back. (Mainly for debugging purposes).")]
    [SerializeField] bool _isKnockbackable;

    [Tooltip("Layer(s) the enemy considers as walkable.")]
    [SerializeField] LayerMask _walkableLayer;

    [Tooltip("How long (in seconds) must pass before the enemy can recover.")]
    [SerializeField] float _knockbackRecoverTime = 3f;

    //----------

    // Variables
    private bool _isKnocked = false;
    private float _knockBackTime = 0f;

    // References
    Transform target; // Changed target to search for the player in awake [Tegomlee]
    NavMeshAgent navMeshAgent;
    Animator animator;
    CapsuleCollider capsuleCollider;

    private void Awake()
    {
        // Search for the player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Assign component references
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (!_isKnocked)
            EngageTarget();
        
        // Check if enemy can recover
        else
        {
            // Increment the time the enemy is knocked out
            _knockBackTime += Time.deltaTime;

            // Perform recover check
            if (_knockBackTime >= _knockbackRecoverTime && IsGrounded())
            {
                _isKnocked = false;
                Knockback.Instance.SetComponentState(capsuleCollider, true);
            }
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
        if (navMeshAgent.enabled && !_isKnocked)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetBool("attack", false);
        }
        else return;
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

    // This is the new way the enemy will recover, this is so it can be affected without instantly resetting [Tegomlee]
    private bool IsGrounded()
    {
        Collider[] groundArray = Physics.OverlapSphere(transform.position, 5f, _walkableLayer, QueryTriggerInteraction.Ignore);
        Debug.Log(groundArray.Length);
        return groundArray.Length > 0;
    }

    public void PrepareEnemyForKnockback()
    {
        _knockBackTime = 0f;
        _isKnocked = true;
    }

    // Controls the wire mesh that determines how close player
    // must be for enemy to chase player
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
