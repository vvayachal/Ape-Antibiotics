using UnityEngine;

public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, EnemyBehaviour enemyBehaviour) : base(needsExitTime, enemyBehaviour) { }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        Debug.Log("Idling!");
        //animator.Play("Idle");
    }
}
