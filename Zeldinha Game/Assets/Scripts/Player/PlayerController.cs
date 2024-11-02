using System.Collections.Generic;
using UnityEngine;
using Player.States;
public class PlayerController : MonoBehaviour
{
    // Public fields
    [Header("Jump")]
    public float JumpPower = 10;
    public float JumpMovementFactor = 1f;
    [Tooltip("Distance of the raycast to detect the ground")]
    public float downRayDistance = 0.0f;
    public GameObject jumpEffectPrefab;
    public AudioClip jumpSound;
    public AudioClip landSound;

    [Header("Movement")]
    public float MovementSpeed = 30f;
    public float MaxSpeed = 6f;


    // StateMachine
    public StateMachine stateMachine;
    public Idle idleState;
    public Walking walkingState;
    public Jumping jumpingState;
    public Dead deadState;
    public Attack attackState;
    public Defend defendState;
    public Hurt hurtState;

    // Internal fields
    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider col;
    [HideInInspector] public Life lifeScript;
    [HideInInspector] public bool hasJumpInput;
    private AudioSource audioSource;

    // Slope
    private bool isOnSlope;
    [HideInInspector] public Vector3 slopeNormal;

    // Attack
    [Header("Attack")]
    public int attackStages = 3;
    public List<float> attackStageDurations;
    public List<float> attackStageMaxIntervals;
    public List<float> attackStageImpulses;
    public List<int> attackDamagePerStage;
    public GameObject swordHitBox;
    [SerializeField] private float swordKnockbackImpulse;
    [SerializeField] private GameObject swordHitEffect;
    [SerializeField] private AudioClip swordAttackAudio;
    [SerializeField] float maxDistanceToLook = 5.0f;


    // Defend
    [Header("Defend")]
    public GameObject shieldHitbox;
    [SerializeField] private float shieldKnockbackImpulse;
    [HideInInspector] public bool hasDefenseInput;
    public GameObject defendEffectPrefab;
    public AudioClip defendSound;

    // Hurt
    [Header("Hurt")]
    public float hurtDuration = 0.2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();


        lifeScript = GetComponent<Life>();
        lifeScript.OnDamage += OnDamage;
        lifeScript.OnHeal += OnHeal;
        lifeScript.canTakeDamage += CanTakeDamage;

    }
    private void Start()
    {
        stateMachine = new();
        idleState = new(this);
        walkingState = new(this);
        jumpingState = new(this);
        deadState = new(this);
        attackState = new(this);
        defendState = new(this);
        hurtState = new(this);
        stateMachine.ChangeState(idleState);

        swordHitBox.SetActive(false);

        swordHitEffect.transform.position = swordHitBox.transform.position;
        swordHitEffect.SetActive(false);

        // UI
        GameManager.Instance.gameplayUI.playerHealthBar.SetMaxHealth(lifeScript.maxHealth);
    }

    private void Update()
    {
        // Read Input
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        hasJumpInput = Input.GetKey(KeyCode.Space);

        hasDefenseInput = Input.GetMouseButton(1);

        // Create movement vector
        float inputX = isRight ? 1f : isLeft ? -1f : 0f;
        float inputY = isUp ? 1f : isDown ? -1f : 0f;
        movementVector = new(inputX, inputY);

        float speed = rb.velocity.magnitude;
        float speedRate = speed / MaxSpeed;
        anim.SetFloat("fVelocity", speedRate);


        if (GameManager.Instance.bossBattleHandler.IsInCutscene())
        {
            if (stateMachine.CurrentStateName != idleState.name)
            {
                stateMachine.ChangeState(idleState);
            }
            return;
        }
        stateMachine.Update();

        DetectSlope();
    }

    private void FixedUpdate()
    {
        // State Machine
        stateMachine.FixedUpdate();

        // Apply gravity
        Vector3 gravityForce = Physics.gravity * (isOnSlope ? 0f : 1f);
        rb.AddForce(gravityForce, ForceMode.Acceleration);

        // Limit speed
        LimitSpeed();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossRoomSensor"))
        {
            GlobalEvents.Instance.BossRoomEnter();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            lifeScript.TakeDamage(gameObject, lifeScript.maxHealth);
        }
    }

    public Quaternion GetCameraRotation()
    {
        Camera cam = Camera.main;
        float eulerY = cam.transform.eulerAngles.y;
        return Quaternion.Euler(0, eulerY, 0);
    }

    public void RotateBodyToFaceInput(float alpha = 0.3f)
    {
        if (movementVector.IsZero()) return;

        // Calculate rotation
        Camera cam = Camera.main;
        Vector3 inputVector = new(movementVector.x, 0, movementVector.y);
        Quaternion q1 = Quaternion.LookRotation(inputVector);
        Quaternion q2 = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, toRotation, alpha);

        // Apply rotation
        rb.MoveRotation(smoothRotation);
    }

    public void FaceClosestEnemy(float alpha)
    {
        RotateBodyToFaceInput(alpha);

        List<GameObject> enemies = GameManager.Instance.enemies;
        if (enemies.Count == 0) return;

        float closestDistance = float.MaxValue;
        Vector3 closestEnemyDirection = Vector3.zero;
        foreach (var enemy in enemies)
        {
            Vector3 enemyDirection = enemy.transform.position - transform.position;
            float distance = enemyDirection.magnitude;
            bool isInRange = distance < maxDistanceToLook;
            bool isInFrontOfPlayer = Vector3.Dot(enemyDirection.normalized, transform.forward) > 0.5f;
            if (isInRange && distance < closestDistance && isInFrontOfPlayer)
            {
                closestDistance = distance;
                closestEnemyDirection = enemyDirection;
            }
        }

        if (closestEnemyDirection.IsZero()) return;
        Camera cam = Camera.main;
        closestEnemyDirection.y = 0;
        Quaternion q1 = Quaternion.LookRotation(closestEnemyDirection);
        Quaternion q2 = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, toRotation, alpha);

        // Apply rotation
        rb.MoveRotation(smoothRotation);
    }

    public bool AttemptToAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool isAttacking = stateMachine.CurrentStateName == attackState.name;
            bool canAttack = !isAttacking || attackState.CanSwitchStages();

            if (canAttack)
            {
                int attackStage = isAttacking ? (attackState.stage + 1) : 1;
                attackState.stage = attackStage;
                stateMachine.ChangeState(attackState);
                audioSource.PlayOneShot(swordAttackAudio);
                return true;
            }
        }
        return false;
    }
    
    public void OnSwordCollisionEnter (Collider other)
    {
        GameObject other_go = other.gameObject;
        bool isTarget = other_go.layer == LayerMask.NameToLayer("Target");
        if (!isTarget) return;

        // Damage
        if (other_go.TryGetComponent(out Life other_life))
        {
            if (other_life.TakeDamage(gameObject, attackDamagePerStage[attackState.stage - 1]))
            {
                PlaySwordHitEffect();
            }
        }

        // Knockback
        if (other_go.TryGetComponent(out Rigidbody other_rb))
        {
            Vector3 positionDiff = other_go.transform.position - transform.position;
            Vector3 impulseVector = new(positionDiff.x, 0.0f, positionDiff.z);
            impulseVector = impulseVector.normalized * swordKnockbackImpulse;
            other_rb.AddForce(impulseVector, ForceMode.Impulse);
        }
    }

    public void OnShieldCollisionEnter(Collider other)
    {
        return;

        GameObject other_go = other.gameObject;

        if (other_go.TryGetComponent(out Rigidbody other_rb))
        {
            Vector3 positionDiff = other_go.transform.position - transform.position;
            Vector3 impulseVector = new(positionDiff.x, 0.0f, positionDiff.z);
            impulseVector = impulseVector.normalized * shieldKnockbackImpulse;
            other_rb.AddForce(impulseVector, ForceMode.Impulse);

            Debug.Log("Collided with " + other_go.name);
        }
    }

    public bool DetectGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, downRayDistance, GameManager.Instance.groundLayer);
    }

    private void DetectSlope()
    {
        isOnSlope = false;
        slopeNormal = Vector3.up;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, downRayDistance))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) < 0.99f)
            {
                slopeNormal = hit.normal;
                isOnSlope = true;
                return;
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, downRayDistance))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) < 0.99f)
            {
                slopeNormal = hit.normal;
                isOnSlope = true;
            }
        }
    }

    private void LimitSpeed()
    {
        if (isOnSlope)
        {
            if (rb.velocity.magnitude > MaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * MaxSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new(rb.velocity.x, 0, rb.velocity.z);
            if (flatVelocity.magnitude > MaxSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * MaxSpeed;
                rb.velocity = new(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void PlaySwordHitEffect()
    {
        if (swordHitEffect.activeSelf)
        {
            swordHitEffect.SetActive(false);

        }
        swordHitEffect.SetActive(true);
    }

    private void OnDamage(object sender, DamageEventArgs e)
    {
        Debug.Log("Player has been damaged by " + e.attacker.name + " with " + e.damage + " damage");
        GameManager.Instance.gameplayUI.playerHealthBar.SetHealth(lifeScript.CurrentHealth);
        stateMachine.ChangeState(hurtState);
    }

    private void OnHeal()
    {
        Debug.Log("Player has received a cure!");
        GameManager.Instance.gameplayUI.playerHealthBar.SetHealth(lifeScript.CurrentHealth);
    }
    private bool CanTakeDamage(GameObject attacker, int damage)
    {
        bool isDefending = stateMachine.CurrentStateName == defendState.name;

        if (!isDefending)
        {
            return true;
        }

        Vector3 attackerDirection = (this.transform.position - attacker.transform.position).normalized;

        if (Vector3.Dot(attackerDirection, this.transform.forward) > -0.25f)
        {
            return true;
        }

        // Create defense effect
        var defendEffect = Instantiate(defendEffectPrefab, shieldHitbox.transform.position, Quaternion.identity);
        Destroy(defendEffect, 1.0f);

        // Play defense sound
        PlaySound(defendSound);

        return false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * downRayDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToLook);
    }
}
