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
    }
}
