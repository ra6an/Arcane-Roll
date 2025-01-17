using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chill : EffectBase, IStatusEffect
{
    private EffectDefaultValues _default = new();

    public Chill()
    {
        Name = "Chill";
        Description = $"Lose {_default.Chill} health at the end of the turn.";
        Type = EffectType.Status;
        Modify = ModifyStat.None;
    }

    public void ApplyStatus(Damageable target)
    {
        target.ApplyStatus(_default.Chill);
    }
}
