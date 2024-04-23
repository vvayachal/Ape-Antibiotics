using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private Transform target;
    public ChaseState(bool needsExitTime, EnemyBehaviour enemyBehaviour, Transform target) : base(needsExitTime, enemyBehaviour) 
    {
        this.target = target;
    }

    public override void OnEnter()
    {
        if (!enemyBehaviour.transform.GetComponent<EnemyManager>().IsDead() || enemyBehaviour.isKnockedBack)
        {
            base.OnEnter();
            agent.enabled = true;
            agent.isStopped = false;
            enemyBehaviour.isKnockedBack = false;
            Debug.Log("Chasing!");
            //animator.Play("Walk");
        }
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if(!requestedExit && !enemyBehaviour.isKnockedBack)
        {
            agent.SetDestination(target.position);
        } else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
