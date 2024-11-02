using UnityEngine;

namespace Player.States
{
    public class Attack : State
    {
        private PlayerController controller;

        public int stage = 1;
        private float stateTime;
        public Attack(PlayerController controller) : base("Attack")
        {
            this.controller = controller;
        }

        public override void Enter()
        {
            base.Enter();
            controller.FaceClosestEnemy(1);
            controller.anim.SetTrigger("tAttack" + stage);
            stateTime = 0;

            // Apply impulse
            float impulseForce = controller.attackStageImpulses[stage - 1];
            Vector3 impulseVector = controller.rb.rotation * Vector3.forward * impulseForce;
            controller.rb.AddForce(impulseVector, ForceMode.Impulse);

            controller.swordHitBox.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

        }

        public override void Update()
        {
            base.Update();

            // Switch to attack
            if (controller.AttemptToAttack())
            {
                return;
            }

            // Update state time
            stateTime += Time.deltaTime;

            if (controller.swordHitBox.activeSelf && IsAttackDone())
            {
                controller.swordHitBox.SetActive(false);
            }

            // Exit after time
            if (IsStageExpired())
            {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }


            if (!controller.movementVector.IsZero() && IsAttackDone())
            {
                controller.stateMachine.ChangeState(controller.walkingState);
                return;
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public bool CanSwitchStages()
        {

            bool isLastStage = stage == controller.attackStages;
            float stageDuration = controller.attackStageDurations[stage - 1];
            float stageMaxInterval = isLastStage ? 0 : controller.attackStageMaxIntervals[stage - 1];
            float maxStageDuration = stageDuration + stageMaxInterval;

            return !isLastStage && stateTime >= stageDuration && stateTime <= maxStageDuration;
        }

        public bool IsStageExpired()
        {
            bool isLastState = stage == controller.attackStages;
            float stageDuration = controller.attackStageDurations[stage - 1];
            float stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
            float maxStageDuration = stageDuration + stageMaxInterval;

            return stateTime > maxStageDuration;
        }

        public bool IsAttackDone()
        {
            var stageDuration = controller.attackStageDurations[controller.attackStages - 1];
            return stateTime > stageDuration;
        }
    }
}
