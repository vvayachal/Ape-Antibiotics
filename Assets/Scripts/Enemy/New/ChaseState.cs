using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private Transform target;
    public ChaseState(bool needsExitTime, Enemy enemy, Transform target) : base(needsExitTime, enemy) 
    {
        this.target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.enabled = true;
        agent.isStopped = false;
        Debug.Log("Chasing!");
        //animator.Play("Walk");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if(!requestedExit)
        {
            agent.SetDestination(target.position);
        } else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}