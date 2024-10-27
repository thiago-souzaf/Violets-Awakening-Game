using UnityEngine;

public class MeleeCreatureHelper
{
	private MeleeCreatureController m_controller;
	public MeleeCreatureHelper(MeleeCreatureController controller)
	{
        m_controller = controller;
    }

	public float GetDistanceToPlayer()
	{
        GameObject player = GameManager.Instance.player;
        Vector3 direction = player.transform.position - m_controller.transform.position;

		return direction.magnitude;
    }

	public bool IsPlayerOnSight()
	{
		GameObject player = GameManager.Instance.player;
		Vector3 direction = player.transform.position - m_controller.transform.position;
		float distance = direction.magnitude;

        // Player is too far
        if (distance > m_controller.searchRadius)
		{
			return false;
		}

		// Check for obstacles
		int searchLayer = LayerMask.GetMask("Player", "Default");
		if (Physics.Raycast(m_controller.transform.position, direction.normalized, out RaycastHit hit,  m_controller.searchRadius, searchLayer))
		{
            if (hit.transform.gameObject != player)
			{
				return false;
			}
		}

		// Player on sight!
        return true;
    }

	public void FacePlayer()
	{
		Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 direction = playerPosition - m_controller.transform.position;
		direction.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        m_controller.transform.rotation = Quaternion.Slerp(m_controller.transform.rotation, targetRotation, 0.1f);
    }

	public void PlayAttackSound(bool hasHit)
	{
		if (m_controller.audioSource == null)
		{
			return;
		}

		if (hasHit && m_controller.attackHitSound)
		{
			m_controller.audioSource.PlayOneShot(m_controller.attackHitSound);
		}
		else if (!hasHit && m_controller.attackMissSound)
		{
			m_controller.audioSource.PlayOneShot(m_controller.attackMissSound);
		}
	}

	public void PlayDeadSound()
	{
		if (m_controller.audioSource != null && m_controller.deadSound != null)
		{
            m_controller.audioSource.PlayOneShot(m_controller.deadSound);
		}
	}
}
