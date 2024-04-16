using UnityHFSM;
using System;

public class AttackState : EnemyStateBase
{
    public AttackState(
        bool needsExitTime,
        Enemy enemy,
        Action<State<EnemyState, StateEvent>> onEnter,
        float exitTime = 0.33f) : base(needsExitTime, enemy, exitTime, onEnter) { }

    public override void OnEnter()
    {
        agent.isStopped = true;
        base.OnEnter();
        animator.Play("Enemy Attack");
    }
}
