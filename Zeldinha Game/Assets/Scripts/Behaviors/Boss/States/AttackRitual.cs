using System.Collections;
using UnityEngine;

namespace Behaviors.Boss.States
{
    public class AttackRitual : State
    {
        private BossController m_controller;

        private float m_attackCooldown;
        public AttackRitual(BossController bossController) : base("AttackRitual")
        {
            m_controller = bossController;
        }

        public override void Enter()
        {
            base.Enter();
            m_attackCooldown = m_controller.attackRitualDuration;
            m_controller.animator.SetTrigger("tAttackRitual");
            m_controller.RegisterCoroutine(ScheduleAttack(m_controller.attackRitualDelay));
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
            GameObject areaOfEffect = Object.Instantiate(m_controller.attackRitualPrefab, m_controller.bottomStaff.position, Quaternion.identity);

            Object.Destroy(areaOfEffect, 10);
        }
    }
}
