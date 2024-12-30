using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RelicSO))]
public class RelicSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RelicSO relic = (RelicSO)target;

        // Ispisivanje osnovnih informacija o relicu
        relic.id = EditorGUILayout.IntField("Id", relic.id);
        relic.relicName = EditorGUILayout.TextField("Relic Name", relic.relicName);
        relic.description =  EditorGUILayout.TextField("Description", relic.description);
        relic.icon = (Sprite)EditorGUILayout.ObjectField("Icon", relic.icon, typeof(Sprite), false);

        // Prikazivanje selektora za tip efekta
        relic.type = (RelicEffectType)EditorGUILayout.EnumFlagsField("Relic Effect Type", relic.type);

        // Dinamicko prikazivanje opcija zavisno od izabranih efekata
        if ((relic.type & RelicEffectType.ExtraRolls) != 0)
        {
            relic.numOfExtraRolls = EditorGUILayout.IntField("Extra Rolls", relic.numOfExtraRolls);
        }

        if ((relic.type & RelicEffectType.Lifesteal) != 0)
        {
            relic.lifestealPercentage = EditorGUILayout.FloatField("Lifesteal Percentage", relic.lifestealPercentage);
        }

        if ((relic.type & RelicEffectType.InvincibleFirstDamage) != 0)
        {
            relic.invincibleFirstDamageTaken = EditorGUILayout.Toggle("Invincible First Damage", relic.invincibleFirstDamageTaken);
        }

        if ((relic.type & RelicEffectType.Thorns) != 0)
        {
            relic.thornsTurns = EditorGUILayout.IntField("Thorns Turns", relic.thornsTurns);
        }

        if ((relic.type & RelicEffectType.BurnBoost) != 0)
        {
            relic.burnPercentage = EditorGUILayout.FloatField("Burn Boost Percentage", relic.burnPercentage);
        }

        if ((relic.type & RelicEffectType.PoisonBoost) != 0)
        {
            relic.poisonPercentage = EditorGUILayout.FloatField("Poison Boost Percentage", relic.poisonPercentage);
        }

        if ((relic.type & RelicEffectType.EnemyDebuffOnBattleStart) != 0)
        {
            relic.debuffType = (Debuff)EditorGUILayout.EnumPopup("Debuff Type", relic.debuffType);
            relic.debuffDuration = EditorGUILayout.IntField("Debuff Duration", relic.debuffDuration);
        }

        if ((relic.type & RelicEffectType.ExtraCoinsByRewards) != 0)
        {
            relic.extraCoinsRewardPercentage = EditorGUILayout.FloatField("Extra Coins Reward Percentage", relic.extraCoinsRewardPercentage);
        }

        if ((relic.type & RelicEffectType.ShieldBoost) != 0)
        {
            relic.shieldBoostTurns = EditorGUILayout.IntField("Shield Boost Turns", relic.shieldBoostTurns);
        }

        if ((relic.type & RelicEffectType.Strength) != 0)
        {
            relic.strengthTurns = EditorGUILayout.IntField("Strength Turns", relic.strengthTurns);
        }

        // Spremanje promena
        if (GUI.changed)
        {
            EditorUtility.SetDirty(relic);
        }

        //base.OnInspectorGUI();
    }
}
