using UnityEngine;

namespace Behaviors.Boss.States
{
    public class Dead : State
    {
        private BossController m_controller;
        public Dead(BossController bossController) : base("Dead")
        {
            m_controller = bossController;
        }

        public override void Enter()
        {
            base.Enter();
            m_controller.animator.SetTrigger("tDead");
            m_controller.thisCollider.enabled = false;
            GlobalEvents.Instance.GameWon();

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
