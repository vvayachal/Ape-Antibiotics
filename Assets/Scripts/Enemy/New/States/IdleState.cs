using UnityEngine;

public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, EnemyBehaviour enemyBehaviour) : base(needsExitTime, enemyBehaviour) { }

    public override void OnEnter()
    {
        if (!enemyBehaviour.transform.GetComponent<EnemyManager>().IsDead() || enemyBehaviour.isKnockedBack)
        {
            base.OnEnter();
            agent.isStopped = true;
            Debug.Log("Idling!");
            //animator.Play("Idle");
        }
    }

    public override void OnLogic()
    {
        base.OnLogic();
    }
}
