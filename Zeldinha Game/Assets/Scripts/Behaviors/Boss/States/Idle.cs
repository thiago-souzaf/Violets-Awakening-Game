using UnityEngine;

namespace Behaviors.Boss.States
{
    public class Idle : State
    {
        private BossController m_controller;

        private float m_stateDuration;
        public Idle(BossController bossController) : base("Idle")
        {
            m_controller = bossController;
        }

        public override void Enter()
        {
            base.Enter();
            m_stateDuration = 0f;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (GameManager.Instance.IsGameOver)
            {
                return;
            }
            m_stateDuration += Time.deltaTime;

            // Switch to follow state
            if (m_stateDuration >= m_controller.idleDuration)
            {
                m_controller.stateMachine.ChangeState(m_controller.followState);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
