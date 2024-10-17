using UnityEngine;

namespace Behaviors.Boss.States
{
    public class Idle : State
    {
        private BossController m_controller;
        public Idle(BossController bossController) : base("Idle")
        {
            m_controller = bossController;
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
}
