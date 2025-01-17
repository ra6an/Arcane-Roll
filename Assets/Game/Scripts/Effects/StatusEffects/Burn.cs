using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : EffectBase, IStatusEffect
{
    private EffectDefaultValues _default = new();

    public Burn()
    {
        Name = "Burn";
        Description = $"Lose {_default.Burn * 100} health at the end of the turn.";
        Type = EffectType.Status;
        Modify = ModifyStat.None;
    }

    public void ApplyStatus(Damageable target)
    {
        target.ApplyStatus(_default.Burn);
    }
}
