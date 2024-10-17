using UnityEngine;

namespace Behaviors.Boss.States
{
    public class AttackSuper : State
    {
        private BossController m_controller;
        public AttackSuper(BossController bossController) : base("AttackSuper")
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
