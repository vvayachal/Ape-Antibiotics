using UnityEngine;
using UnityHFSM;
using Sensors;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public Transform player;

    [Header("Sensors")]
    [SerializeField]
    private PlayerSensor followPlayerSensor;
    [SerializeField]
    private PlayerSensor meleePlayerSensor;

    [Header("Attack Config")]
    [SerializeField]
    [Range(0.1f, 5f)]
    private float attackCooldown = 2;

    [Space]
    [Header("Debug Info")]
    [SerializeField]
    private bool isInMeleeRange;
    [SerializeField]
    private bool isInChaseRange;
    [SerializeField]
    private float lastAttackTime;
    [SerializeField]
    public bool isKnockedBack;

    private StateMachine<EnemyState, StateEvent> EnemyFSM;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        EnemyFSM = new StateMachine<EnemyState, StateEvent>();

        //Add States
        EnemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        EnemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, player.transform));
        EnemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, player.transform, OnAttack));
        EnemyFSM.AddState(EnemyState.Death, new DeathState(false, this));

        //Add Transitions
        EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => isInChaseRange 
            && Vector3.Distance(player.transform.position, transform.position) > agent.stoppingDistance)
        );
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => !isInChaseRange
            || Vector3.Distance(player.transform.position, transform.position) <= agent.stoppingDistance)
        );

        //Attack Transitions
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, forceInstantly: true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, forceInstantly: true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

        EnemyFSM.Init();
    }

    private void Start()
    {
        followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
        followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
        meleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
        meleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;
    }

    private void FollowPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition)
    {
        EnemyFSM.Trigger(StateEvent.LostPlayer);
        isInChaseRange = false;
    }
    private void FollowPlayerSensor_OnPlayerEnter(Transform player)
    {
        EnemyFSM.Trigger(StateEvent.DetectPlayer);
        isInChaseRange = true;
    }

    private void MeleePlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => isInMeleeRange = false;
    private void MeleePlayerSensor_OnPlayerEnter(Transform player) => isInMeleeRange = true;

    private void OnAttack(State<EnemyState, StateEvent> state) 
    { 
        transform.LookAt(player.transform.position);
        lastAttackTime = Time.time;
    }

    private bool IsWithinIdleRange(Transition<EnemyState> transition) =>
        agent.remainingDistance <= agent.stoppingDistance;

    private bool IsNotWithinIdleRange(Transition<EnemyState> transition) =>
        !IsWithinIdleRange(transition);

    private bool ShouldMelee(Transition<EnemyState> transition) =>
        lastAttackTime + attackCooldown <= Time.time
        && isInMeleeRange;

    private void Update()
    {
        EnemyFSM.OnLogic();
    }
}
