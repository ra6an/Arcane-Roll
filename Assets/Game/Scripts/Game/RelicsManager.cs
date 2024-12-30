using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicsManager : MonoBehaviour
{
    private List<RelicSO> playersRelics = new();

    public List<RelicSO> PlayersRelics => playersRelics;

    public void AddRelic(RelicSO _relic)
    {
        playersRelics.Add(_relic);

        SubscribeToRelicEvents(_relic);
    }

    public void RemoveRelic(RelicSO _relic)
    {
        playersRelics.Remove(_relic);

        UnsubscribeToRelicEvents(_relic);
    }

    private void SubscribeToRelicEvents(RelicSO relic)
    {
        if (relic.type.HasFlag(RelicEffectType.ExtraRolls))
        {
            relic.OnExtraRollsActivated += HandleExtraRolls;
        }
        if (relic.type.HasFlag(RelicEffectType.Lifesteal))
        {
            relic.OnLifestealActivated += HandleLifesteal;
        }
        if (relic.type.HasFlag(RelicEffectType.ShieldBoost))
        {
            relic.OnShieldBoostActivated += HandleShieldBoost;
        }
        if (relic.type.HasFlag(RelicEffectType.Strength))
        {
            relic.OnStrengthActivated += HandleStrength;
        }
    }

    private void UnsubscribeToRelicEvents(RelicSO relic)
    {
        if (relic.type.HasFlag(RelicEffectType.ExtraRolls))
        {
            relic.OnExtraRollsActivated -= HandleExtraRolls;
        }
        if (relic.type.HasFlag(RelicEffectType.Lifesteal))
        {
            relic.OnLifestealActivated -= HandleLifesteal;
        }
        if (relic.type.HasFlag(RelicEffectType.ShieldBoost))
        {
            relic.OnShieldBoostActivated -= HandleShieldBoost;
        }
        if (relic.type.HasFlag(RelicEffectType.Strength))
        {
            relic.OnStrengthActivated -= HandleStrength;
        }
    }

    void HandleExtraRolls()
    {
        //Logika za extra rolls
    }

    void HandleLifesteal()
    {
        // Logika za lifesteal
    }

    void HandleShieldBoost()
    {
        // logika za shield boost
    }

    void HandleStrength()
    {
        // logika za strength
    }
}
