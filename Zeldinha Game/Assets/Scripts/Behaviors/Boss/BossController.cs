using UnityEngine;
using UnityEngine.AI;
using Behaviors.Boss.States;
using System.Collections.Generic;
using System.Collections;
public class BossController : MonoBehaviour
{
    // Components
    [HideInInspector] public BossHelper helper;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Life lifeScript;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Collider thisCollider;
    [HideInInspector] public AudioSource audioSource;

    // Debug fields
    [Header("Debug")]
    [SerializeField] private string _currentStateName;

    // State Machine
    public StateMachine stateMachine;
    public Idle idleState;
    public Follow followState;
    public Hurt hurtState;
    public Dead deadState;
    public AttackNormal attackNormalState;
    public AttackSuper attackSuperState;
    public AttackRitual attackRitualState;

    private List<IEnumerator> attackCoroutines = new();

    [Header("General")]
    public float lowHealthThreshold = 0.3f;
    public Transform topStaff;
    public Transform bottomStaff;

    [Header("Idle")]
    public float idleDuration = 0.5f;

    [Header("Follow")]
    public float ceaseFollowInterval = 4f;

    [Header("Attack")]
    public int attackDamage = 1;
    public GameObject attackNormalPrefab;
    public GameObject attackSuperPrefab;
    public GameObject attackRitualPrefab;

    [Header("Attack Normal")]
    public float attackNormalDelay = 1f;
    public float attackNormalDuration = 1f;
    public float attackNormalImpulse = 3;
    public Vector3 aimOffset;

    [Header("Attack Ritual")]
    public float distanceToRitualAttack = 1f;
    public float attackRitualDelay = 0f;
    public float attackRitualExplosionDelay = 0.5f;
    public float attackRitualDuration = 1f;
    public float attackRitualRadius = 1f;
    public AudioClip attackRitualExplosionSound;

    [Header("Attack Super")]
    public int attackSuperMagicCount = 5;
    public float attackSuperMagicDelay = 0f;
    public float attackSuperMagicDuration = 1f;

    [Header("Hurt")]
    public float hurtDuration = 1f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lifeScript = GetComponent<Life>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        helper = new BossHelper(this);
        thisCollider = GetComponent<Collider>();

    }

    private void Start()
    {
        idleState = new Idle(this);
        followState = new Follow(this);
        hurtState = new Hurt(this);
        deadState = new Dead(this);
        attackNormalState = new AttackNormal(this);
        attackSuperState = new AttackSuper(this);
        attackRitualState = new AttackRitual(this);

        stateMachine = new StateMachine();

        stateMachine.ChangeState(idleState);

        lifeScript.OnDamage += OnDamage;

        // UI
        GameManager.Instance.gameplayUI.bossHealthBar.SetMaxHealth(lifeScript.maxHealth);
    }

    private void Update()
    {


        if (GameManager.Instance.bossBattleHandler.IsBossBattling())
        {
            stateMachine.Update();
        }

        _currentStateName = stateMachine.CurrentStateName;

        float speedRate = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("fSpeed", speedRate);

        // Face player
        if (!lifeScript.IsDead())
        {
            Vector3 vecToPlayer = GameManager.Instance.player.transform.position - transform.position;
            vecToPlayer.y = 0;
            vecToPlayer.Normalize();

            Quaternion desiredRotation = Quaternion.LookRotation(vecToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 0.2f);
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    private void OnDamage(object sender, DamageEventArgs e)
    {
        Debug.Log("Creature has been damaged by " + e.attacker.name + " with " + e.damage + " damage");

        stateMachine.ChangeState(hurtState);
        // UI
        GameManager.Instance.gameplayUI.bossHealthBar.SetHealth(lifeScript.CurrentHealth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToRitualAttack);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bottomStaff.position, attackRitualRadius);
    }

    public void RegisterCoroutine(IEnumerator coroutine)
    {
        attackCoroutines.Add(coroutine);
        StartCoroutine(coroutine);
    }

    public void CancelAllScheduledAttacks()
    {
        foreach (var coroutine in attackCoroutines)
        {
            StopCoroutine(coroutine);
        }
        attackCoroutines.Clear();
    }
}
