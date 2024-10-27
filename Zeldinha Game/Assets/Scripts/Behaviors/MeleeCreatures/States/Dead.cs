using UnityEngine;

namespace Behaviors.MeleeCreatures.States
{
    public class Dead : State
    {
        private MeleeCreatureController m_controller;
        private MeleeCreatureHelper m_helper;
        public Dead(MeleeCreatureController controller) : base("Dead")
        {
            m_controller = controller;
            m_helper = controller.helper;
        }

        public override void Enter()
        {
            // Pause damage
            m_controller.lifeScript.isVunerable = false;
            base.Enter();

            // Set animation trigger
            m_controller.animator.SetTrigger("tDead");

            // Deactivate Collider
            m_controller.thisCollider.enabled = false;

            // Play dead sound
            m_helper.PlayDeadSound();
        }

        public override void Exit()
        {
            base.Exit();
        }
        public override void Update()
        {
            base.Update();

            // Destroy if too far
            if (m_helper.GetDistanceToPlayer() > m_controller.destroyIfFar)
            {
                Object.Destroy(m_controller.gameObject);
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
