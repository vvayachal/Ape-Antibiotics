using UnityEngine;

public class DeathState : EnemyStateBase
{
    public DeathState(bool needsExitTime, EnemyBehaviour enemyBehaviour) : base(needsExitTime, enemyBehaviour) { }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        Debug.Log("dying...");
        //animator.Play("Death");
    }
}
