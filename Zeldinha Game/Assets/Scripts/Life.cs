using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int maxHealth;
    public int CurrentHealth { get; private set; }

    public bool isVunerable = true;

    public event EventHandler<DamageEventArgs> OnDamage;

    public event Action OnHeal;
    public event Action OnDeath;

    public delegate bool CanTakeDamage(GameObject attacker, int damage);
    public event CanTakeDamage canTakeDamage;


    [SerializeField] private GameObject m_healEffect;  

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public bool TakeDamage(GameObject attacker, int damage)
    {
        if (!isVunerable)
        {
            return false;
        }

        if (canTakeDamage != null && !canTakeDamage(attacker, damage))
        {
            return false;
        }

        CurrentHealth -= damage;
        OnDamage?.Invoke(sender: this, e: new DamageEventArgs
        {
            damage = damage,
            attacker = attacker
        });

        if (IsDead())
        {
            OnDeath?.Invoke();
        }
        return true;
    }
    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    public void Heal()
    {
        CurrentHealth = maxHealth;
        
        var effect = Instantiate(m_healEffect, transform.position, m_healEffect.transform.rotation);
        effect.transform.SetParent(transform);
        Destroy(effect, 5.0f);

        OnHeal?.Invoke();
    }

    public float GetHealthRate()
    {
        return (float)CurrentHealth / maxHealth;
    }
}