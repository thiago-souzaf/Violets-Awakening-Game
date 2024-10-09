using System.Collections;
using UnityEngine;

public class CreatureAttack : State
{
    private MeleeCreatureController m_controller;
    private MeleeCreatureHelper m_helper;

    private float endAttackCooldown;
    private IEnumerator attackCoroutine;

    public CreatureAttack(MeleeCreatureController controller) : base("Attack")
    {
        m_controller = controller;
        m_helper = controller.helper;
    }

    public override void Enter()
    {
        base.Enter();
        endAttackCooldown = m_controller.attackDuration;

        attackCoroutine = ScheduleAttack();
        m_controller.StartCoroutine(attackCoroutine);
    }

    public override void Exit()
    {
        base.Exit();

        // Cancel attack
        if (attackCoroutine != null)
        {
            m_controller.StopCoroutine(attackCoroutine);
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

    public override void Update()
    {
        base.Update();

        if ((endAttackCooldown -= Time.deltaTime) <= 0)
        {
            m_controller.stateMachine.ChangeState(m_controller.idleState);
            return;
        }
    }

    private IEnumerator ScheduleAttack()
    {
        yield return new WaitForSeconds(m_controller.damageDelay);
        PerformAttack();
    }

    private void PerformAttack()
    {
        if (Physics.SphereCast(m_controller.transform.position, m_controller.attackRadius, m_controller.transform.forward, out RaycastHit hitInfo, m_controller.attackSphereRadius))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                // Perform Attack!
                // TODO: 
                // controller.attackDamage (int)
            }
        }
    }
}
