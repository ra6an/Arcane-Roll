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

    public void Heal(AbilityExecutionContext aec)
    {
        int amount = aec.lethal ? aec.amount * 2 : aec.amount;

        if(currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        } else
        {
            currentHealth += amount;
        }
    }

    public void TakeDamage(AbilityExecutionContext aec)
    {
        int amount = aec.amount;
        Debug.Log($"Damage is: {amount}");
        if (aec.lethal)
        {
            bool isInLethalRange = IsInLethalRange();

            amount = isInLethalRange ? amount * 2 : amount;
            Debug.Log($"Damage is lethal: {amount}");
        }

        
        int damageAfterShield = aec.piercing ? amount : TakeShieldDamage(amount);

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
            AbilityExecutionContext aec = new()
            {
                amount = -_amount
            };

            AddShield(aec);
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

    public void AddShield(AbilityExecutionContext aec)
    {
        int amount = aec.lethal ? aec.amount * 2 : aec.amount;

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

    private bool IsInLethalRange()
    {
        int amount = (int)Mathf.Ceil(maxHealth * 0.4f);
        bool isLethal = false;

        if (currentHealth <= amount) isLethal = true;

        return isLethal;
    }
}
