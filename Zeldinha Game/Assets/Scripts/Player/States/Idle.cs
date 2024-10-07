using UnityEngine;
public class Idle : State
{
    private PlayerController controller;
    public Idle(PlayerController controller) : base("Idle")
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

        // Switch to jumping
        if (controller.hasJumpInput)
        {
            controller.stateMachine.ChangeState(controller.jumpingState);
        }

        // Switch to walking
        if (!controller.movementVector.IsZero())
        {
            controller.stateMachine.ChangeState(controller.walkingState);
            return;
        }

        // Switch to defending
        if (controller.hasDefenseInput)
        {
            controller.stateMachine.ChangeState(controller.defendState);
            return;
        }

        controller.rb.velocity = Vector3.Lerp(controller.rb.velocity, new(0, controller.rb.velocity.y, 0), 0.1f);
    }
}
