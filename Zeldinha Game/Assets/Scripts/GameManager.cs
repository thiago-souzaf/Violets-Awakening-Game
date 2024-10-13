using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Physics")]
    public LayerMask groundLayer;
    public GameObject player;

    public List<Interaction> interatbleObjects;
    private void Start()
    {
    }

    private void Update()
    {
		
    } 

    public Interaction GetClosestInteraction(Vector3 position)
    {
        Interaction closestInteraction = null;
        float closestDistance = float.MaxValue;

        foreach (Interaction interaction in interatbleObjects)
        {
            float sqrDistance = (position - interaction.transform.position).sqrMagnitude;
            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                closestInteraction = interaction;
            }
        }
        return closestInteraction;
    }
}
