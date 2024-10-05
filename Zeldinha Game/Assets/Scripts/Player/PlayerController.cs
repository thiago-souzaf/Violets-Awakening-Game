using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public fields
    public float MovementSpeed = 10f;
    public float JumpPower = 10;
    public float JumpMovementFactor = 1f;
    public LayerMask platformsLayer;

    // StateMachine
    public StateMachine stateMachine;
    public Idle idleState;
    public Walking walkingState;
    public Jumping jumpingState;
    public Dead deadState;

    // Internal fields
    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider col;
    [HideInInspector] public bool hasJumpInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }
    private void Start()
    {
        stateMachine = new();
        idleState = new(this);
        walkingState = new(this);
        jumpingState = new(this);
        deadState = new(this);
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver && stateMachine.CurrentStateName != deadState.name)
        {
            stateMachine.ChangeState(deadState);
        }

        // Read Input
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        hasJumpInput = Input.GetKey(KeyCode.Space);

        // Create movement vector
        float inputX = isRight ? 1f : isLeft ? -1f : 0f;
        float inputY = isUp ? 1f : isDown ? -1f : 0f;
        movementVector = new(inputX, inputY);

        float speed = rb.velocity.magnitude;
        float speedRate = speed / MovementSpeed;
        anim.SetFloat("fVelocity", speedRate);

        stateMachine.Update();
        
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
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

    public void RotateBodyToFaceInput()
    {
        if (movementVector.IsZero()) return;

        // Calculate rotation
        Camera cam = Camera.main;
        Vector3 inputVector = new(movementVector.x, 0, movementVector.y);
        Quaternion q1 = Quaternion.LookRotation(inputVector);
        Quaternion q2 = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, toRotation, 0.15f);

        // Apply rotation
        rb.MoveRotation(smoothRotation);
    }

    public bool DetectGround()
    {
        Vector3 origin = transform.position;
        Bounds bounds = col.bounds;

        float radius = bounds.size.x * 0.25f;

        return Physics.CheckSphere(origin, radius, platformsLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Bounds bounds = col.bounds;

        float radius = bounds.size.x * 0.25f;
        Gizmos.DrawSphere(origin , radius);
    }
}
