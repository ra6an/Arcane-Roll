using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brittle : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Brittle()
    {
        Name = "Brittle";
        Description = $"You receive {_default.Brittle * 100}% less shield.";
        Type = EffectType.Debuff;
        Modify = ModifyStat.ReceiveDamage;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Brittle;
        int modifiedAmount = 0;

        modifiedAmount = -(int)Mathf.Ceil(_amount * _defaultAmount);

        return modifiedAmount;
    }
}
