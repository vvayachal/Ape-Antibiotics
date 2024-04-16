using UnityHFSM;
using System;
using UnityEngine;

public class AttackState : EnemyStateBase
{
    private Transform target;

    public AttackState(
        bool needsExitTime,
        EnemyBehaviour enemyBehaviour,
        Transform target,
        Action<State<EnemyState, StateEvent>> onEnter,
        float exitTime = 0.33f) : base(needsExitTime, enemyBehaviour, exitTime, onEnter) { }

    public override void OnEnter()
    {
        agent.isStopped = true;
        base.OnEnter();
        animator.Play("Enemy Attack");
        target.GetComponentInParent<Shield>().TakeDamage(10f);
        Debug.Log($"{this.name} has attacked {target.name}");
    }
}
