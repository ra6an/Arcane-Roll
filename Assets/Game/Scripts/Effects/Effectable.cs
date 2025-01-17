using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effectable : MonoBehaviour
{
    private List<EffectBase> effects = new();

    public void AddNewEffect(EffectBase _effToAdd, int _duration)
    {
        if (_effToAdd == null || _duration <= 0) return;
        Debug.Log($"Prije dodavanja: {effects.Count} -- EFEKAT: {_effToAdd.Name} ({_effToAdd.Duration}) --");
        EffectBase _eff = effects.Find(x => x.Name == _effToAdd.Name);

        if(_eff != null)
        {
            _eff.Add(_duration);
            Debug.Log($"Poslije dodavanja: {effects.Count} -- EFEKAT: {_eff.Name} ({_eff.Duration}) --");
            return;
        }

        _effToAdd.Add(_duration);
        effects.Add(_effToAdd);

        Debug.Log($"Poslije dodavanja: {effects.Count} -- EFEKAT: {_effToAdd.Name} ({_effToAdd.Duration}) --");
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
        Debug.Log(effects.Count);
        foreach (var effect in effects)
        {
            if(effect is IBuffDebuffEffect buffDebuff && effect.Modify == ModifyStat.ReceiveDamage)
            {
                Debug.Log($"EFEKAT: {effect.Name}, MODIFIKUJE: {effect.Modify}");
                Debug.Log("Modifikuje amount!!!");
                modifiedAmount += buffDebuff.Apply(amount);
            }
        }

        return modifiedAmount;
    }
}
