using UnityEngine;

namespace Behaviors.Boss.States
{
    public class Hurt : State
    {
        private BossController m_controller;
        private BossHelper m_helper;
        private float m_stateTime;
        private bool attackedOnLastHurt = false;
        public Hurt(BossController bossController) : base("Hurt")
        {
            m_controller = bossController;
            m_helper = m_controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            if (m_controller.lifeScript.IsDead())
            {
                m_controller.stateMachine.ChangeState(m_controller.deadState);
                return;
            }

            m_stateTime = 0;

            // Pause damage
            m_controller.lifeScript.isVunerable = false;

            // Set animation trigger
            m_controller.animator.SetTrigger("tHurt");

            if (!attackedOnLastHurt)
            {
                m_helper.InstatiateAreaOfEffect(m_controller.attackRitualPrefab, m_controller.attackRitualExplosionDelay);
            }

            attackedOnLastHurt = !attackedOnLastHurt;
        }

        public override void Exit()
        {
            base.Exit();
            m_controller.lifeScript.isVunerable = true;

        }

        public override void Update()
        {
            base.Update();

            if ((m_stateTime += Time.deltaTime) >= m_controller.hurtDuration)
            {
                m_controller.stateMachine.ChangeState(m_controller.idleState);
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
