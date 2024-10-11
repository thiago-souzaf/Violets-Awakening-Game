using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int maxHealth;
    private int m_currentHealth;

    public bool isVunerable = true;

    public event EventHandler<DamageEventArgs> OnDamage;

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
}