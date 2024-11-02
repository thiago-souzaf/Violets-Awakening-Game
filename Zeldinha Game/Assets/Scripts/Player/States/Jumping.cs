using UnityEngine;
namespace Player.States
{
    public class Jumping : State
    {
        PlayerController controller;
        private bool hasJumped;
        private float cooldown;
        public Jumping(PlayerController controller) : base("Jumping")
        {
            this.controller = controller;
        }

        public override void Enter()
        {
            base.Enter();

            hasJumped = false;
            cooldown = 0.5f;

            controller.anim.SetBool("bIsJumping", true);

            // Create effect
            var jumpEffect = Object.Instantiate(controller.jumpEffectPrefab, controller.transform.position, Quaternion.identity);
            Object.Destroy(jumpEffect, 1f);

            // Play jumping sound
            controller.PlaySound(controller.jumpSound);
        }

        public override void Exit()
        {
            base.Exit();
            controller.anim.SetBool("bIsJumping", false);

            // Create effect
            var jumpEffect = Object.Instantiate(controller.jumpEffectPrefab, controller.transform.position, Quaternion.identity);
            Object.Destroy(jumpEffect, 1f);

            // Play landing sound
            controller.PlaySound(controller.landSound);

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Debug.Log($"hasJumped: {hasJumped} | isGrounded: {controller.isGrounded}");
            if (!hasJumped)
            {
                hasJumped = true;
                Jump();
            }

            // Create movement vector
            Vector3 walkVector = new Vector3(controller.movementVector.x, 0, controller.movementVector.y);
            walkVector = controller.GetCameraRotation() * walkVector;
            walkVector *= controller.MovementSpeed * controller.JumpMovementFactor;

            // Apply input to character
            controller.rb.AddForce(walkVector);

            controller.RotateBodyToFaceInput();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }

        public override void Update()
        {
            base.Update();

            cooldown -= Time.deltaTime;
            if (hasJumped && controller.DetectGround() && cooldown <= 0f)
            {
                controller.stateMachine.ChangeState(controller.idleState);
            }
        }

        private void Jump()
        {
            Vector3 forceVector = Vector3.up * controller.JumpPower;
            controller.rb.AddForce(forceVector, ForceMode.Impulse);
        }
    }
}
