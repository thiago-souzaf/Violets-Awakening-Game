public class CreatureDead : State
{
    private MeleeCreatureController m_controller;
    private MeleeCreatureHelper m_helper;
    public CreatureDead(MeleeCreatureController controller) : base("Dead")
    {
        m_controller = controller;
        m_helper = controller.helper;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
