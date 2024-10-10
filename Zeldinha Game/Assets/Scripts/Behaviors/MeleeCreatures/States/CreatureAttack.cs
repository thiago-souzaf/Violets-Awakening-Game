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
        Vector3 origin = m_controller.transform.position + m_controller.transform.forward * m_controller.attackRadius;
        int playerLayer = LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(origin, m_controller.attackSphereRadius, playerLayer);

        foreach (Collider collider in colliders)
        {
            Debug.Log("Hit object: " + collider.gameObject.name);
            if (collider.TryGetComponent(out Life lifeScript))
            {
                lifeScript.TakeDamage(m_controller.gameObject, m_controller.attackDamage);
            }
        }
    }
}
