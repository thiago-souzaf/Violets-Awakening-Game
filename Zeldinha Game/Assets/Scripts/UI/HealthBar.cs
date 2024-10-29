using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Slider slider;
	private int maxHealth;
    private int currentHealth;


	public void SetMaxHealth(int maxHealth)
	{
		this.maxHealth = maxHealth;
        slider.maxValue = this.maxHealth;
		SetHealth(maxHealth);
    }

    public void SetHealth(int health)
	{
		currentHealth = health;
		slider.value = currentHealth;
    }
}
