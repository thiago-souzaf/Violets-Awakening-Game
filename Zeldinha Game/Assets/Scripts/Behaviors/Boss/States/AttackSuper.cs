using System.Collections;
using UnityEngine;

namespace Behaviors.Boss.States
{
    public class AttackSuper : State
    {
        private float m_attackCooldown;

        private BossController m_controller;
        private BossHelper m_helper;

        public AttackSuper(BossController bossController) : base("AttackSuper")
        {
            m_controller = bossController;
            m_helper = m_controller.helper;

        }

        public override void Enter()
        {
            base.Enter();
            m_attackCooldown = m_controller.attackSuperMagicDuration;
            m_controller.animator.SetTrigger("tAttackSuper");

            float attackSuperStep = (m_controller.attackSuperMagicDuration - m_controller.attackSuperMagicDelay) / (m_controller.attackSuperMagicCount - 1);
            Debug.Log("Step attack = " + attackSuperStep);

            for (int i = 0; i < m_controller.attackSuperMagicCount; i++)
            {
                float delay = m_controller.attackSuperMagicDelay + (i * attackSuperStep);
                m_controller.RegisterCoroutine(ScheduleAttack(delay));
                Debug.Log("Delay = " + delay);
            }


        }

        public override void Exit()
        {
            base.Exit();
            m_controller.CancelAllScheduledAttacks();

        }
        public override void Update()
        {
            base.Update();
            if ((m_attackCooldown -= Time.deltaTime) <= 0f)
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

        private IEnumerator ScheduleAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            PerformAttack();
        }

        private void PerformAttack()
        {
            m_helper.InstatiateProjectile(m_controller.attackSuperPrefab);
        }

    }
}
