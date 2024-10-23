using UnityEngine;

namespace Behaviors.Boss.States
{
    public class Follow : State
    {
        private readonly BossController m_controller;
        private readonly BossHelper m_helper;

        private readonly float m_attackAttemptInterval = 1f;
        private float m_attackAttemptCooldown = 0f;

        private float m_ceaseFollowCooldown = 0f;

        private readonly float m_targetUpdateInterval = 0.3f;
        private float m_targetUpdateCooldown = 0f;

        public Follow(BossController bossController) : base("Follow")
        {
            m_controller = bossController;
            m_helper = m_controller.helper;
        }

        public override void Enter()
        {
            base.Enter();
            m_attackAttemptCooldown = m_attackAttemptInterval;
            m_ceaseFollowCooldown = m_controller.ceaseFollowInterval;
            m_targetUpdateCooldown = 0f;
        }

        public override void Exit()
        {
            base.Exit();
            m_controller.agent.ResetPath();
        }
        public override void Update()
        {
            base.Update();


            // Update destination
            if ((m_targetUpdateCooldown -= Time.deltaTime) <= 0f)
            {
                m_targetUpdateCooldown = m_targetUpdateInterval;

                Vector3 playerPosition = GameManager.Instance.player.transform.position;
                m_controller.agent.SetDestination(playerPosition);
            }

            // Attempt to attack
            if ((m_attackAttemptCooldown -= Time.deltaTime) <= 0)
            {
                m_attackAttemptCooldown = m_attackAttemptInterval;

                // Ritual
                float distanceToPlayer = m_helper.GetDistanceToPlayer();
                if (distanceToPlayer <= m_controller.distanceToRitualAttack)
                {
                    m_controller.stateMachine.ChangeState(m_controller.attackRitualState);
                    return;
                }
            }

            // Attempt to cease follow
            if ((m_ceaseFollowCooldown -= Time.deltaTime) <= 0f)
            {
                m_ceaseFollowCooldown = m_controller.ceaseFollowInterval;

                State newState = m_helper.HasLowHealth() ? m_controller.attackSuperState : m_controller.attackNormalState;
                m_controller.stateMachine.ChangeState(newState);
                return;
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
