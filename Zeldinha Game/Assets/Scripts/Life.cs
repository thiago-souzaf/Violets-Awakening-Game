using UnityEngine;

public class Life : MonoBehaviour
{
	public int maxHealth;
	private int currentHealth;
    [SerializeField] private GameObject DestroyFX;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameObject destroyFx = Instantiate(DestroyFX, transform.position, transform.rotation);
            Destroy(destroyFx, 2f);
            Destroy(gameObject);
        }
    }
}
