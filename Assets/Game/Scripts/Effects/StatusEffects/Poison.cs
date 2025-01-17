using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : EffectBase, IStatusEffect
{
    private EffectDefaultValues _default = new();

    public Poison()
    {
        Name = "Poison";
        Description = $"Lose {_default.Poison} health at the end of the turn.";
        Type = EffectType.Status;
        Modify = ModifyStat.None;
    }

    public void ApplyStatus(Damageable target)
    {
        target.ApplyStatus(_default.Poison);
    }
}
