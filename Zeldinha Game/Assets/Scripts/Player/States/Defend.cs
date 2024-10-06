public class Defend : State
{
    private PlayerController controller;
    public Defend(PlayerController controller) : base("Defend")
    {
        this.controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        controller.anim.SetBool("bDefend", true);
        controller.shieldHitbox.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        controller.anim.SetBool("bDefend", false);
        controller.shieldHitbox.SetActive(false);

    }

    public override void Update()
    {
        base.Update();

        if (!controller.hasDefenseInput)
        {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }

        controller.RotateBodyToFaceInput(0.5f);
    }
}
