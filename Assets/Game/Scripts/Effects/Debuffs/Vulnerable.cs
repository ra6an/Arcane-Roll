using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vulnerable : EffectBase, IBuffDebuffEffect
{
    private EffectDefaultValues _default = new();

    public Vulnerable()
    {
        Name = "Vulnerable";
        Description = $"You take {_default.Vulnerable * 100}% more damage.";
        Type = EffectType.Debuff;
        Modify = ModifyStat.ReceiveDamage;
    }

    public int Apply(int _amount)
    {
        float _defaultAmount = _default.Vulnerable;
        int modifiedAmount = 0;

        modifiedAmount = (int)Mathf.Ceil(_amount * _defaultAmount);

        return modifiedAmount;
    }
}
