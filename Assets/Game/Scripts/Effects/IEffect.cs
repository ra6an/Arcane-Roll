using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Buff,
    Debuff,
    CrowdControl,
    Status
}

public enum ModifyStat
{
    None,
    Attack,
    ReceiveDamage,
    Defense,
    ReceiveDefense,
    Heal,
    ReceiveHeal
}

public interface IEffect
{
    string Name { get; }
    string Description { get; }
    int Duration { get; } 
    EffectType Type { get; }
    ModifyStat Modify { get; }

    //int Apply(int _amount);
}

public interface IBuffDebuffEffect : IEffect
{
    int Apply(int _amount);
}

public interface IStatusEffect : IEffect
{
    void ApplyStatus(Damageable target);
}

public interface ICrowdControlEffect : IEffect
{
    void ApplyCrowdControl();
}
