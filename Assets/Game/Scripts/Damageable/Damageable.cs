using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    private List<EffectBase> effects = new();
    private BattleSettings _battleSettings;
    private CombatAnimatorController _combatAnimator;
    private Effectable _effectable;
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
        _effectable = GetComponent<Effectable>();
        _battleSettings = GameManager.Instance.BattleSettings;
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

    public void Lifesteal(int _damage)
    {
        int healthToRecover = (int)Mathf.Ceil(_damage * _battleSettings.Lifesteal);
        
        if (currentHealth + healthToRecover > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthToRecover;
        }
    }

    public void TakeDamage(AbilityExecutionContext aec)
    {
        int amount = _effectable.ModifyReceiveDamage(aec.amount);

        //Debug.Log($"Damage to take: {amount} --OLD ({aec.amount})--.");
        
        if (aec.lethal)
        {
            bool isInLethalRange = IsInLethalRange();

            amount = isInLethalRange ? amount * 2 : amount;
        }
        
        int damageAfterShield = aec.piercing ? amount : TakeShieldDamage(amount);

        if (currentHealth - damageAfterShield <= 0)
        {
            HandleDie();
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
        bool inLethalRange = IsInLethalRange();
        int amount = aec.lethal && inLethalRange ? aec.amount * 2 : aec.amount;

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
        if( _combatAnimator != null )
        {
            _combatAnimator.DieAnimation();
        }
    }

    private bool IsInLethalRange()
    {
        int amount = (int)Mathf.Ceil(maxHealth * _battleSettings.LethalRange);
        bool isLethal = false;

        if (currentHealth <= amount) isLethal = true;

        return isLethal;
    }

    public void AddEffect(EffectBase _effToAdd, int _duration)
    {
        if (_effToAdd == null || _duration <= 0) return;

        _effectable.AddNewEffect(_effToAdd, _duration);
    }

    public int ModifyAttack(int value)
    {
        return _effectable.ModifyAttackValue(value);
    }

    internal void ApplyStatus(int statusDmg)
    {
        if (currentHealth - statusDmg <= 0)
        {
            HandleDie();
        }
        else
        {
            currentHealth -= statusDmg;
            if (_combatAnimator != null)
            {
                _combatAnimator.TakeDamageAnimation();
            }
        }
    }

    public void HandleEndTurnEffects()
    {
        _effectable.HandleEndTurnEffects();
    }

    private void HandleDie()
    {
        currentHealth = 0;
        Die();
        if (transform.gameObject.layer == LayerMask.NameToLayer("Ally"))
        {
            AllyCrystalController acc = transform.GetComponent<AllyCrystalController>();
            acc.Dissolve();
        }
    }
}
