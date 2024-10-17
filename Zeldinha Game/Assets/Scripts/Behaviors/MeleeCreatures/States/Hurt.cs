using UnityEngine;
namespace Behaviors.MeleeCreatures.States
{
    public class Hurt : State
    {
        private MeleeCreatureController m_controller;
        private MeleeCreatureHelper m_helper;

        private float m_timePassed;
        public Hurt(MeleeCreatureController controller) : base("Hurt")
        {
            m_controller = controller;
            m_helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            if (m_controller.lifeScript.IsDead())
            {
                m_controller.stateMachine.ChangeState(m_controller.deadState);
                return;
            }

            m_timePassed = 0;

            // Pause damage
            m_controller.lifeScript.isVunerable = false;

            // Set animation trigger
            m_controller.animator.SetTrigger("tHurt");
        }

        public override void Exit()
        {
            base.Exit();

            m_controller.lifeScript.isVunerable = true;
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

            m_timePassed += Time.deltaTime;

            if (m_timePassed >= m_controller.hurtDuration)
            {
                m_controller.stateMachine.ChangeState(m_controller.idleState);
                return;
            }
        }
    }
}
