using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilitySO))]
public class AbilitySOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AbilitySO ability = (AbilitySO)target;

        // Osnovne informacije
        ability.id = EditorGUILayout.IntField("ID", ability.id);
        ability.abilityName = EditorGUILayout.TextField("Ability Name", ability.abilityName);
        ability.icon = (Sprite)EditorGUILayout.ObjectField("Icon", ability.icon, typeof(Sprite), false);
        ability.description = EditorGUILayout.TextField("Description", ability.description);

        // Tip ability-ja
        ability.type = (AbilityType)EditorGUILayout.EnumFlagsField("Ability Type", ability.type);

        // Sekcija za Attack
        if (ability.type.HasFlag(AbilityType.Attack))
        {
            EditorGUILayout.LabelField("Attack Settings", EditorStyles.boldLabel);
            ability.attack = EditorGUILayout.IntField("Attack Amount", ability.attack);
            ability.numOfTargetsToAttack = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToAttack);
            ability.numOfAttacks = EditorGUILayout.IntField("Number of Attacks", ability.numOfAttacks);
            ability.lifesteal = EditorGUILayout.Toggle("Lifesteal", ability.lifesteal);
            ability.execute = EditorGUILayout.Toggle("Execute", ability.execute);
            ability.piercing = EditorGUILayout.Toggle("Piercing", ability.piercing);
            ability.attackRandom = EditorGUILayout.Toggle("Attack Random Targets", ability.attackRandom);
            ability.applyEffectToTarget = EditorGUILayout.Toggle("Apply Effect To Target", ability.applyEffectToTarget);
            //ability.canApplyStatusEffect = EditorGUILayout.Toggle("Can Apply Status Effect", ability.canApplyStatusEffect);
        }

        // Sekcija za Defense
        if (ability.type.HasFlag(AbilityType.Defense))
        {
            EditorGUILayout.LabelField("Defense Settings", EditorStyles.boldLabel);
            ability.defense = EditorGUILayout.IntField("Defense Amount", ability.defense);
            ability.numOfTargetsToDefense = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToDefense);
            ability.lethalDefenseBoost = EditorGUILayout.Toggle("Lethal Defense Boost", ability.lethalDefenseBoost);
            ability.defenseRandom = EditorGUILayout.Toggle("Defense Random Targets", ability.defenseRandom);
        }

        // Sekcija za Crowd Control
        if (ability.type.HasFlag(AbilityType.CrowdControl))
        {
            EditorGUILayout.LabelField("Crowd Control Settings", EditorStyles.boldLabel);
            ability.crowdControlType = (CC)EditorGUILayout.EnumPopup("Crowd Control Type", ability.crowdControlType);
            ability.numOfTargetsToCC = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToCC);
            ability.ccDuration = EditorGUILayout.IntField("Duration", ability.ccDuration);
            ability.ccRandom = EditorGUILayout.Toggle("CC Random Targets", ability.ccRandom);
        }

        // Sekcija za Buff
        if (ability.type.HasFlag(AbilityType.Buff))
        {
            EditorGUILayout.LabelField("Buff Settings", EditorStyles.boldLabel);
            ability.buff = (Buff)EditorGUILayout.EnumFlagsField("Buff Type", ability.buff);
            ability.numOfTargetsToBuff = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToBuff);
            ability.buffDuration = EditorGUILayout.IntField("Duration", ability.buffDuration);
            ability.buffRandom = EditorGUILayout.Toggle("Buff Random Targets", ability.buffRandom);
            ability.applyDebuffOnSameTarget = EditorGUILayout.Toggle("Apply Debuff On Same Target", ability.applyDebuffOnSameTarget);
        }

        // Sekcija za Debuff
        if (ability.type.HasFlag(AbilityType.Debuff))
        {
            EditorGUILayout.LabelField("Debuff Settings", EditorStyles.boldLabel);
            ability.debuff = (Debuff)EditorGUILayout.EnumFlagsField("Debuff Type", ability.debuff);
            ability.numOfTargetsToDebuff = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToDebuff);
            ability.debuffDuration = EditorGUILayout.IntField("Duration", ability.debuffDuration);
            ability.debuffRandom = EditorGUILayout.Toggle("Debuff Random Targets", ability.debuffRandom);
        }

        // Sekcija za Status
        if(ability.type.HasFlag(AbilityType.Status))
        {
            EditorGUILayout.LabelField("Status Effects Settings", EditorStyles.boldLabel);
            ability.status = (Status)EditorGUILayout.EnumFlagsField("Status Effect Type", ability.status);
            ability.numOfTargetsToStatus = (NumOfTargets)EditorGUILayout.EnumPopup("Number Of Targets", ability.numOfTargetsToStatus);
            ability.chanceToApplyStatus = EditorGUILayout.FloatField("Chance to Apply Status Effect", ability.chanceToApplyStatus);
            ability.statusDuration = EditorGUILayout.IntField("Duration", ability.statusDuration);
            ability.statusRandom = EditorGUILayout.Toggle("Status Random Target", ability.statusRandom);
        }

        // Sekcija za Heal
        if (ability.type.HasFlag(AbilityType.Heal))
        {
            EditorGUILayout.LabelField("Heal Settings", EditorStyles.boldLabel);
            ability.heal = EditorGUILayout.IntField("Heal Amount", ability.heal);
            ability.numOfTargetsToHeal = (NumOfTargets)EditorGUILayout.EnumPopup("Number of Targets", ability.numOfTargetsToHeal);
            ability.lethalHealBoost = EditorGUILayout.Toggle("Lethal Heal Boost", ability.lethalHealBoost);
            ability.healRandom = EditorGUILayout.Toggle("Heal Random Targets", ability.healRandom);
        }

        // Spremanje izmjena
        if (GUI.changed)
        {
            EditorUtility.SetDirty(ability);
        }
    }
}
