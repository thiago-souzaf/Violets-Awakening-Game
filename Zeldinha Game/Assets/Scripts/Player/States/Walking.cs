using UnityEngine;

public class Walking : State
{
    private PlayerController controller;
    public Walking(PlayerController controller) : base("Walking")
    {
        this.controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Switch to attack
        if (controller.AttemptToAttack())
        {
            return;
        }

        if (controller.hasJumpInput)
        {
            controller.stateMachine.ChangeState(controller.jumpingState);
        }

        if (controller.movementVector.IsZero())
        {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Create movement vector
        Vector3 walkVector = new(controller.movementVector.x, 0, controller.movementVector.y);
        walkVector = controller.GetCameraRotation() * walkVector;
        walkVector = Vector3.ProjectOnPlane(walkVector, controller.slopeNormal);
        walkVector *= controller.MovementSpeed;

        // Apply input to character
        controller.rb.AddForce(walkVector);

        controller.RotateBodyToFaceInput();
    }

}
