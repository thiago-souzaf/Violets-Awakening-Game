using UnityEngine;
using UnityEngine.AI;

public class MeleeCreatureController : MonoBehaviour
{
    [HideInInspector] public MeleeCreatureHelper helper;
    [HideInInspector] public NavMeshAgent agent;


    // Debug fields
    [Header("Debug")]
    [SerializeField] private string _currentStateName;

    // State Machine
    public StateMachine stateMachine;
    public CreatureIdle idleState;
    public CreatureFollow followState;
    public CreatureAttack attackState;
    public CreatureHurt hurtState;
    public CreatureDead deadState;

    [Header("General")]
    public float searchRadius = 5f;

    [Header("Idle")]
    public float targetSearchInterval = 1f;

    [Header("Follow")]
    public float ceaseFollowInterval = 4f;

    [Header("Attack")]
    public float distanceToAttack = 1f;
    public float attackRadius = 1.5f;
    public float attackSphereRadius = 1.5f;
    public float damageDelay = 1f;
    public float attackDuration = 1f;
    public int attackDamage = 1;

    [Header("Hurt")]
    public float hurtDuration = 1f;

    [Header("Dead")]
    public float destroyIfFar = 30f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        helper = new MeleeCreatureHelper(this);

    }

    private void Start()
    {
        idleState = new CreatureIdle(this);
        followState = new CreatureFollow(this);
        attackState = new CreatureAttack(this);
        hurtState = new CreatureHurt(this);
        deadState = new CreatureDead(this);

        stateMachine = new StateMachine();

        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        stateMachine.Update();
        _currentStateName = stateMachine.CurrentStateName;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackSphereRadius);
    }
}
