using UnityEngine;
namespace Behaviors.MeleeCreatures.States
{
    public class Follow : State
    {
        private MeleeCreatureController m_controller;
        private MeleeCreatureHelper m_helper;

        private float updateCooldown;
        private float ceaseFollowCooldown;

        public Follow(MeleeCreatureController controller) : base("Follow")
        {
            m_controller = controller;
            m_helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();
            updateCooldown = 0f;
            ceaseFollowCooldown = m_controller.ceaseFollowInterval;
        }

        public override void Exit()
        {
            base.Exit();
            m_controller.agent.ResetPath();
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

            // Update destination
            if ((updateCooldown -= Time.deltaTime) <= 0f)
            {
                updateCooldown = m_controller.targetSearchInterval;
                Vector3 playerPosition = GameManager.Instance.player.transform.position;
                m_controller.agent.SetDestination(playerPosition);
            }

            // Cease Follow
            if ((ceaseFollowCooldown -= Time.deltaTime) <= 0f)
            {
                ceaseFollowCooldown = m_controller.ceaseFollowInterval;
                if (!m_helper.IsPlayerOnSight())
                {
                    m_controller.stateMachine.ChangeState(m_controller.idleState);
                }
            }

            // Attempt to attack
            if (m_helper.GetDistanceToPlayer() <= m_controller.distanceToAttack)
            {
                m_controller.stateMachine.ChangeState(m_controller.attackState);
                return;
            }
        }
    }
}
