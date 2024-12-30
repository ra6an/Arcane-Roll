using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum RelicEffectType
{
    None = 0,
    ExtraRolls = 1 << 0,
    Lifesteal = 1 << 1,
    InvincibleFirstDamage = 1 << 2,
    Thorns = 1 << 3,
    BurnBoost = 1 << 4,
    PoisonBoost = 1 << 5,
    EnemyDebuffOnBattleStart = 1 << 6,
    ExtraCoinsByRewards = 1 << 7,
    ShieldBoost = 1 << 8,
    Strength = 1 << 9,

}

[CreateAssetMenu(menuName = "Data/Relic")]
public class RelicSO : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string relicName;
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
    [SerializeField] public RelicEffectType type;

    // Public Delegates
    public event Action OnExtraRollsActivated;
    public event Action OnLifestealActivated;
    public event Action OnShieldBoostActivated;
    public event Action OnStrengthActivated;

    // Extra Rolls
    public int numOfExtraRolls;

    // Lifesteal
    public float lifestealPercentage;

    // Invincible first damage
    public bool invincibleFirstDamageTaken;

    // Thorns
    public int thornsTurns;

    // Burn boost
    public float burnPercentage;

    // Poison boost
    public float poisonPercentage;

    // Enemy debuff on start of the battle
    public Debuff debuffType;
    public int debuffDuration;

    // Extra coins by rewards
    public float extraCoinsRewardPercentage;

    // Shield Boost
    public int shieldBoostTurns;

    // Strength Boost
    public int strengthTurns;

    // EVENT METODE ZA RAZLICITE EFFECT-E
    public void ActivateRelevantEvents()
    {
        if(type.HasFlag(RelicEffectType.ExtraRolls) && OnExtraRollsActivated != null)
        {
            OnExtraRollsActivated?.Invoke();
        }

        if (type.HasFlag(RelicEffectType.ShieldBoost) && OnShieldBoostActivated != null)
        {
            OnLifestealActivated?.Invoke();
        }

        if (type.HasFlag(RelicEffectType.Strength) && OnStrengthActivated != null)
        {
            OnStrengthActivated?.Invoke();
        }
    }
}
