using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public fields
    [Header("Jump")]
    public float JumpPower = 10;
    public float JumpMovementFactor = 1f;
    [Tooltip("Distance of the raycast to detect the ground")]
    public float downRayDistance = 0.0f;

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

    // Internal fields
    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider col;
    [HideInInspector] public Life lifeScript;
    [HideInInspector] public bool hasJumpInput;

    // Slope
    private bool isOnSlope;
    [HideInInspector] public Vector3 slopeNormal;

    // Attack
    [Header("Attack")]
    public int attackStages = 3;
    public List<float> attackStageDurations;
    public List<float> attackStageMaxIntervals;
    public List<float> attackStageImpulses;
    public GameObject swordHitBox;
    [SerializeField] private float swordKnockbackImpulse;

    // Defend
    [Header("Defend")]
    [HideInInspector] public bool hasDefenseInput;
    public GameObject shieldHitbox;
    [SerializeField] private float shieldKnockbackImpulse;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();


        lifeScript = GetComponent<Life>();
        lifeScript.OnDamage += OnDamage;
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
        stateMachine.ChangeState(idleState);

        swordHitBox.SetActive(false);
        shieldHitbox.SetActive(false);

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
                return true;
            }
        }
        return false;
    }
    
    public void OnSwordCollisionEnter (Collider other)
    {
        GameObject other_go = other.gameObject;
        bool isTarget = other_go.layer == LayerMask.NameToLayer("Target");

        if (isTarget && other_go.TryGetComponent(out Rigidbody other_rb))
        {
            Vector3 positionDiff = other_go.transform.position - transform.position;
            Vector3 impulseVector = new(positionDiff.x, 0.0f, positionDiff.z);
            impulseVector = impulseVector.normalized * swordKnockbackImpulse;
            other_rb.AddForce(impulseVector, ForceMode.Impulse);
        }
    }

    public void OnShieldCollisionEnter(Collider other)
    {
        GameObject other_go = other.gameObject;
        bool isTarget = true;

        if (isTarget && other_go.TryGetComponent(out Rigidbody other_rb))
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

    private void OnDamage(object sender, DamageEventArgs e)
    {
        Debug.Log("Player has been damaged by " + e.attacker.name + " with " + e.damage + " damage");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * downRayDistance);
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
}
