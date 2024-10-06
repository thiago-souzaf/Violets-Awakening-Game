using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
	public PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        controller.OnSwordCollisionEnter(other);
    }
}
