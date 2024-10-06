using UnityEngine;

public class ShieldHitbox : MonoBehaviour
{
	public PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        controller.OnShieldCollisionEnter(other);
    }
}
