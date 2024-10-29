using UnityEngine;
namespace Behaviors.MeleeCreatures.States
{
    public class Idle : State
    {
        private MeleeCreatureController m_controller;
        private MeleeCreatureHelper m_helper;

        private float searchCooldown;
        public Idle(MeleeCreatureController controller) : base("Idle")
        {
            m_controller = controller;
            m_helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();
            searchCooldown = m_controller.targetSearchInterval;
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

            if (GameManager.Instance.IsGameOver)
            {
                return;
            }

            searchCooldown -= Time.deltaTime;
            if (searchCooldown <= 0)
            {
                searchCooldown = m_controller.targetSearchInterval;
                if (m_helper.IsPlayerOnSight())
                {
                    m_controller.stateMachine.ChangeState(m_controller.followState);
                    return;
                }
            }
        }
    }
}
