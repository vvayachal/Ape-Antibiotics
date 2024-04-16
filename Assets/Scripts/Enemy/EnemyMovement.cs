using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    Animator animator;

    [SerializeField] float chaseRange = 5f;
    // Use Mathf.Infinity so that enemyAI
    // doesn't instantiate thinking that its distanceToTarget is 0
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    [SerializeField] float turnSpeed = 1f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    void EngageTarget()
    {
        //FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void FaceTarget()
    {
        //direction variable of type vector3 gives us the direction that we want this gameobject to rotate with magnitude of 1 (.normalized)
        Vector3 direction = (target.position - transform.position).normalized;
        //lookRotation variable of type quaternion gives us the rotation that we want this gameobject to make. where do we need to rotate essentially. 
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //Quaternion.Slerp lets us rotate smoothly (spherical interpolation) between 2 vectors. 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
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

    // Controls the wire mesh that determines how close player
    // must be for enemy to chase player
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
