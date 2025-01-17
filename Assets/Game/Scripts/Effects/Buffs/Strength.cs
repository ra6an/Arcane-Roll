using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Strength : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Strength()
    {
        Name = "Strength";
        Description = $"Your attack deal {_default.Strength * 100}% more damage.";
        Type = EffectType.Buff;
        Modify = ModifyStat.Attack;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Strength;
        int modifiedAmount = 0;

        modifiedAmount = (int)Mathf.Ceil(_amount * _defaultAmount);
        
        return modifiedAmount;
    }
}
