using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Vitality()
    {
        Name = "Vitality";
        Description = $"You gain {_default.Vitality * 100}% more shield.";
        Type = EffectType.Buff;
        Modify = ModifyStat.ReceiveDefense;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Vitality;
        int modifiedAmount = 0;

        modifiedAmount = (int)Mathf.Ceil(_amount * _defaultAmount);

        return modifiedAmount;
    }
}
