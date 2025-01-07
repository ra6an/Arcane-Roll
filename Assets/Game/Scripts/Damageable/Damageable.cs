using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    private CombatAnimatorController _combatAnimator;
    [VInspector.ReadOnly]
    [SerializeField]
    private int maxHealth;
    [VInspector.ReadOnly]
    [SerializeField]
    private int currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        _combatAnimator = GetComponent<CombatAnimatorController>();
    }

    public void Heal(int amount)
    {
        if(currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        } else
        {
            currentHealth = amount;
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            currentHealth -= amount;
            if(_combatAnimator != null)
            {
                _combatAnimator.TakeDamageAnimation();
            }
        }
    }

    public void SetCurrentHealth(int amount)
    {
        maxHealth = amount;
        currentHealth = amount;
        //Debug.Log(currentHealth.ToString());
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        if( _combatAnimator != null )
        {
            _combatAnimator.DieAnimation();
        }
    }
}
