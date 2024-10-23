using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int maxHealth;
    [SerializeField] private int m_currentHealth;

    public bool isVunerable = true;

    public event EventHandler<DamageEventArgs> OnDamage;

    [SerializeField] private GameObject m_healEffect;  

    private void Start()
    {
        m_currentHealth = maxHealth;
    }

    public void TakeDamage(GameObject attacker, int damage)
    {
        if (!isVunerable)
        {
            return;
        }
        m_currentHealth -= damage;
        OnDamage?.Invoke(sender: this, e: new DamageEventArgs
        {
            damage = damage,
            attacker = attacker
        });
    }
    public bool IsDead()
    {
        return m_currentHealth <= 0;
    }

    public void Heal()
    {
        m_currentHealth = maxHealth;
        
        var effect = Instantiate(m_healEffect, transform.position, m_healEffect.transform.rotation);
        effect.transform.SetParent(transform);
        Destroy(effect, 5.0f);
    }

    public float GetHealthPercentage()
    {
        return (float)m_currentHealth / maxHealth;
    }
}