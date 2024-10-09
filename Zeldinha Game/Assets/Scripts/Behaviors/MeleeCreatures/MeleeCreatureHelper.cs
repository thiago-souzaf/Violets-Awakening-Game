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
		int defaultLayer = LayerMask.GetMask("Default");
		if (Physics.Raycast(m_controller.transform.position, direction.normalized, out RaycastHit hit,  m_controller.searchRadius, defaultLayer))
		{
            if (hit.transform.gameObject != player)
			{
				return false;
			}
		}

		// Player on sight!
        return true;
    }
}


