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
    private int currentShield;
    public int MaxHealth => maxHealth;
    public int CurrentShield => currentShield;

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
        int damageAfterShield = TakeShieldDamage(amount);

        if (currentHealth - damageAfterShield <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            currentHealth -= damageAfterShield;
            if(_combatAnimator != null)
            {
                _combatAnimator.TakeDamageAnimation();
            }
        }
    }

    private int TakeShieldDamage(int _amount)
    {
        int unblockedDamage = 0;

        if(_amount > currentShield)
        {
            unblockedDamage = _amount - currentShield;
            ClearShield();
            return unblockedDamage;
        }

        if (currentShield > _amount)
        {
            AddShield(-_amount);
            return unblockedDamage;
        }

        if(currentShield == _amount)
        {
            ClearShield();
            return unblockedDamage;
        }

        return unblockedDamage;
    }

    public int GetShield()
    {
        return currentShield;
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
    }

    public void ClearShield()
    {
        currentShield = 0;
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
