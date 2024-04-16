using UnityHFSM;
using System;
using UnityEngine.AI;
using UnityEngine;

public abstract class EnemyStateBase : State<EnemyState, StateEvent>
{
    protected readonly EnemyBehaviour enemyBehaviour;
    protected readonly NavMeshAgent agent;
    protected readonly Animator animator;
    protected bool requestedExit;
    protected float exitTime;

    protected readonly Action<State<EnemyState, StateEvent>> onEnter;
    protected readonly Action<State<EnemyState, StateEvent>> onLogic;
    protected readonly Action<State<EnemyState, StateEvent>> onExit;
    protected readonly Func<State<EnemyState, StateEvent>, bool> canExit;

    public EnemyStateBase(bool needsExitTime,
        EnemyBehaviour enemyBehaviour,
        float ExitTime = 0.1f,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        Action<State<EnemyState, StateEvent>> onLogic = null,
        Action<State<EnemyState, StateEvent>> onExit = null,
        Func<State<EnemyState, StateEvent>, bool> canExit = null)
    {
        this.enemyBehaviour = enemyBehaviour;
        this.onEnter = onEnter;
        this.onLogic = onLogic;
        this.onExit = onExit;
        this.canExit = canExit;
        this.exitTime = ExitTime;
        this.needsExitTime = needsExitTime;
        agent = enemyBehaviour.GetComponent<NavMeshAgent>();
        animator = enemyBehaviour.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        requestedExit = false;
        onEnter?.Invoke(this);
    }

    public override void OnLogic() 
    { 
        base.OnLogic();
        if(requestedExit && timer.Elapsed >= exitTime) 
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if(!needsExitTime || canExit != null && canExit(this))
        {
            fsm.StateCanExit();
        }
        else
        {
            requestedExit = true;
        }
    }

}
