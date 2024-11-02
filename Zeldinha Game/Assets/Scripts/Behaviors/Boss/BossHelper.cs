using System.Collections;
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

    public void InstatiateAreaOfEffect(GameObject aoePrefab, float explosionDelay)
    {
        GameObject areaOfEffect = Object.Instantiate(aoePrefab, m_controller.bottomStaff.position, Quaternion.identity);

        Object.Destroy(areaOfEffect, 5);

        m_controller.StartCoroutine(DealAreaOfEffectDamage(areaOfEffect.transform.position, explosionDelay));
    }

    private IEnumerator DealAreaOfEffectDamage(Vector3 aoePosition, float explosionDelay)
    {
        yield return new WaitForSeconds(explosionDelay);

        var playerLayer = LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(aoePosition, m_controller.attackRitualRadius, playerLayer);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<Life>().TakeDamage(m_controller.gameObject, m_controller.attackDamage);
            }
        }

        // Play explosion sound
        m_controller.audioSource.PlayOneShot(m_controller.attackRitualExplosionSound);
    }

    
    public void PlayDeathSequence()
    {
        GameObject deathSequenceEffect = GameManager.Instance.bossDeathSequencePrefab;
        Object.Instantiate(deathSequenceEffect, m_controller.transform.position, Quaternion.identity);
    }
}
