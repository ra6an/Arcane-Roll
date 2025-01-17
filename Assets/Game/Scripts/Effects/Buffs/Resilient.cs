using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resilient : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Resilient()
    {
        Name = "Resilient";
        Description = $"You receive {_default.Resilient * 100}% less damage.";
        Type = EffectType.Buff;
        Modify = ModifyStat.ReceiveDamage;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Resilient;
        int modifiedAmount = 0;

        modifiedAmount = -(int)Mathf.Ceil(_amount * _defaultAmount);

        return modifiedAmount;
    }
}
