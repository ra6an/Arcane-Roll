using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effectable : MonoBehaviour
{
    private List<EffectBase> effects = new();

    public List<EffectBase> Effects => effects;

    public void AddNewEffect(EffectBase _effToAdd, int _duration)
    {
        if (_effToAdd == null || _duration <= 0) return;
        EffectBase _eff = effects.Find(x => x.Name == _effToAdd.Name);

        if(_eff != null)
        {
            _eff.Add(_duration);
            return;
        }

        _effToAdd.Add(_duration);
        effects.Add(_effToAdd);
    }

    internal void HandleEndTurnEffects()
    {
        foreach (EffectBase _eff in effects)
        {
            if(_eff.Type == EffectType.Status && _eff is IStatusEffect _statusEff)
            {
                _statusEff.ApplyStatus(transform.GetComponent<Damageable>());
            }
            
            _eff.Tick();
        }

        effects.RemoveAll(_eff => _eff.Duration == 0);
    }

    internal int ModifyAttackValue(int value)
    {
        if(value <= 0) return 0;

        int modifiedValue = value;

        foreach (var effect in effects)
        {
            if(effect is IBuffDebuffEffect buffDebuff && effect.Modify == ModifyStat.Attack)
            {
                modifiedValue += buffDebuff.Apply(value);
            }
        }

        return modifiedValue;
    }

    internal int ModifyReceiveDamage(int amount)
    {
        if(amount <= 0) return 0;

        int modifiedAmount = amount;
        //Debug.Log(effects.Count);
        foreach (var effect in effects)
        {
            if(effect is IBuffDebuffEffect buffDebuff && effect.Modify == ModifyStat.ReceiveDamage)
            {
                //Debug.Log($"EFEKAT: {effect.Name}, MODIFIKUJE: {effect.Modify}");
                //Debug.Log("Modifikuje amount!!!");
                modifiedAmount += buffDebuff.Apply(amount);
            }
        }

        return modifiedAmount;
    }

    public bool EffectExistInList(string effName)
    {
        bool exist = false;

        EffectBase eb = effects.Find(x => x.Name == effName);
        if (eb != null)
        {
            exist = true;
        }

        if(eb == null)
        {
            Debug.Log(effName);
        }

        return exist;
    }
}
