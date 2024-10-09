using UnityEngine;
using UnityEngine.AI;
public class AIExemple : MonoBehaviour
{
	public Transform player;
	private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(player.position);
    }
}
