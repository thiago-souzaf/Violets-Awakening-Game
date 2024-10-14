using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
	private float m_scanInterval = 0.1f;
	private float m_scanTimer = 0.0f;

    private Interaction m_currentInteraction;

    private void Update()
    {
        if ((m_scanTimer -= Time.deltaTime) <= 0.0f)
        {
            m_scanTimer = m_scanInterval;
            ScanForInteractableObjects();
        }

        // Process input
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_currentInteraction?.Interact();
            ScanForInteractableObjects();
        }
    }

    private void ScanForInteractableObjects()
    {
        Interaction closestInteraction = GetClosestInteraction(transform.position);
        if (closestInteraction == m_currentInteraction)
        { return; }

        m_currentInteraction?.SetActive(false);
        closestInteraction?.SetActive(true);
        m_currentInteraction = closestInteraction;
    }

    public Interaction GetClosestInteraction(Vector3 position)
    {
        Interaction closestInteraction = null;
        float closestDistance = float.MaxValue;
        List<Interaction> interactableObjects = GameManager.Instance.interactableObjects;

        foreach (Interaction interaction in interactableObjects)
        {
            float dist2 = (position - interaction.transform.position).sqrMagnitude;
            bool isInRange = dist2 <= interaction.interactionRadius * interaction.interactionRadius;
            if (interaction.isAvailable && isInRange && dist2 < closestDistance)
            {
                closestDistance = dist2;
                closestInteraction = interaction;
            }
        }
        return closestInteraction;
    }
}
