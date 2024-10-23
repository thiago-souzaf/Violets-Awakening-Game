using System.Collections;
using UnityEngine;

namespace Behaviors.Boss.States
{
    public class AttackNormal : State
    {
        private BossController m_controller;
        private BossHelper m_helper;

        private float m_attackCooldown;
        public AttackNormal(BossController bossController) : base("AttackNormal")
        {
            m_controller = bossController;
            m_helper = m_controller.helper;
        }

        public override void Enter()
        {
            base.Enter();
            m_attackCooldown = m_controller.attackNormalDuration;
            m_controller.animator.SetTrigger("tAttackNormal");
            m_controller.RegisterCoroutine(ScheduleAttack(m_controller.attackNormalDelay));

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
            m_helper.InstatiateProjectile(m_controller.attackNormalPrefab);
        }
    }
}
