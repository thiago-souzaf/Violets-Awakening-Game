using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public GameObject hitEffect;

    [HideInInspector] public GameObject attacker;
    [HideInInspector] public int damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Life>().TakeDamage(attacker, damage);
        }

        Debug.Log("Collided with: " + collision.gameObject.name);
        // Instantiate hit effect
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.0f);

        Destroy(gameObject);
    }
}