using UnityEngine;

namespace Player.States
{
    public class Hurt : State
    {
        private PlayerController controller;
        private float stateTime;

        public Hurt(PlayerController controller) : base("Hurt")
        {
            this.controller = controller;
        }



        public override void Enter()
        {
            base.Enter();

            // Check if dead
            if (controller.lifeScript.CurrentHealth <= 0f)
            {
                controller.stateMachine.ChangeState(controller.deadState);
                return;
            }
            // Set animation trigger
            controller.anim.SetTrigger("tHurt");

            // Pause damage
            controller.lifeScript.isVunerable = false;

            stateTime = 0f;
        }

        public override void Exit()
        {
            base.Exit();

            // Resume damage
            controller.lifeScript.isVunerable = true;
        }

        public override void Update()
        {
            base.Update();

            if ((stateTime += Time.deltaTime) >= controller.hurtDuration)
            {
                controller.stateMachine.ChangeState(controller.idleState);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

    }
}
