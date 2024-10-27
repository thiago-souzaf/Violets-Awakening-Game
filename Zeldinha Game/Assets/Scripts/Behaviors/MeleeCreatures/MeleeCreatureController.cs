using UnityEngine;
using UnityEngine.AI;
using Behaviors.MeleeCreatures.States;
public class MeleeCreatureController : MonoBehaviour
{
    // Components
    [HideInInspector] public MeleeCreatureHelper helper;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Life lifeScript;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider thisCollider;

    // Debug fields
    [Header("Debug")]
    [SerializeField] private string _currentStateName;

    // State Machine
    public StateMachine stateMachine;
    public Idle idleState;
    public Follow followState;
    public Attack attackState;
    public Hurt hurtState;
    public Dead deadState;

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

    [Header("Audio")]
    public AudioClip attackHitSound;
    public AudioClip attackMissSound;
    public AudioClip deadSound;
    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lifeScript = GetComponent<Life>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        helper = new MeleeCreatureHelper(this);
        thisCollider = GetComponent<Collider>();

    }

    private void Start()
    {
        idleState = new Idle(this);
        followState = new Follow(this);
        attackState = new Attack(this);
        hurtState = new Hurt(this);
        deadState = new Dead(this);

        stateMachine = new StateMachine();

        stateMachine.ChangeState(idleState);

        lifeScript.OnDamage += OnDamage;
    }

    private void Update()
    {
        stateMachine.Update();
        _currentStateName = stateMachine.CurrentStateName;

        float speedRate = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("fSpeed", speedRate);
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

    private void OnDamage(object sender, DamageEventArgs e)
    {
        Debug.Log("Creature has been damaged by " + e.attacker.name + " with " + e.damage + " damage");

        stateMachine.ChangeState(hurtState);
    }
}
