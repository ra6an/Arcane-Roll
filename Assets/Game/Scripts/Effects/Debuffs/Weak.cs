using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weak : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Weak()
    {
        Name = "Weak";
        Description = $"You deal {_default.Weak * 100}% less damage.";
        Type = EffectType.Debuff;
        Modify = ModifyStat.Attack;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Weak;
        int modifiedAmount = 0;

        modifiedAmount = -(int)Mathf.Ceil(_amount * _defaultAmount);

        return modifiedAmount;
    }
}
