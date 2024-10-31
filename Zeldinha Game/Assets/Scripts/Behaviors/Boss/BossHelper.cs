using UnityEngine;

public class BossHelper
{
	private BossController m_controller;
	public BossHelper(BossController bossController)
	{
		m_controller = bossController;
	}

    public float GetDistanceToPlayer()
    {
        GameObject player = GameManager.Instance.player;
        Vector3 direction = player.transform.position - m_controller.transform.position;

        return direction.magnitude;
    }

    public bool HasLowHealth()
    {
        return m_controller.lifeScript.GetHealthRate() <= m_controller.lowHealthThreshold;
    }

    public void InstatiateProjectile(GameObject projectilePrefab)
    {
        GameObject projectile = Object.Instantiate(projectilePrefab, m_controller.topStaff.position, m_controller.topStaff.rotation);

        projectile.GetComponent<ProjectileCollision>().attacker = m_controller.gameObject;
        projectile.GetComponent<ProjectileCollision>().damage = m_controller.attackDamage;

        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 impulseDirection = m_controller.topStaff.forward;
        Vector3 directionToPlayer = ((playerPosition + m_controller.aimOffset) - m_controller.topStaff.position).normalized;
        impulseDirection.y = directionToPlayer.y;
        impulseDirection *= m_controller.attackNormalImpulse;

        projectile.GetComponent<Rigidbody>().AddForce(impulseDirection, ForceMode.Impulse);

        Object.Destroy(projectile, 10);
    }

    public void PlayDeathSequence()
    {
        GameObject deathSequenceEffect = GameManager.Instance.bossDeathSequencePrefab;
        Object.Instantiate(deathSequenceEffect, m_controller.transform.position, Quaternion.identity);
    }
}
