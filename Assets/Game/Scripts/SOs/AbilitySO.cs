using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum AbilityType
{
    None,
    Attack,
    Defense,
    CrowdControl,
    Buff,
    Debuff,
    Heal,
}

public enum NumOfTargets
{
    None,
    One,
    Two,
    Three,
    All,
}

public enum CC
{
    None,
    Stun,
    Charm,
    Confuse,
}

public enum Buff
{
    None,
    Strength,
    Vitality,
    Immortal,
    Unstoppable,
    Piercing,
    Divinity,
}

public enum Debuff
{
    None,
    Vulnerable,
    Weak,
    Shatter,
    Depressed,
}

[CreateAssetMenu(menuName = "Data/Ability")]
public class AbilitySO : ScriptableObject, IAbility
{
    public string abilityName;
    public Sprite icon;
    public string description;
    public AbilityType type;
    [Header("Attack")]
    public int attack;
    public NumOfTargets numOfTargetsToAttack;
    public int numOfAttacks;
    public bool lifesteal = false;
    public bool execute = false;
    public bool attackRandom = false;
    [Header("Defense")]
    public int defense;
    public NumOfTargets numOfTargetsToDefense;
    public bool defenseRandom = false;
    public bool lethalBoost = false;
    [Header("Crowd Control")]
    public CC crowdControlType;
    public NumOfTargets numOfTargetsToCC;
    public int ccDuration = 1;
    public bool ccRandom = false;
    [Header("Buff")]
    public Buff buff;
    public NumOfTargets numOfTargetsToBuff;
    public int buffDuration = 1;
    public bool buffRandom = false;
    [Header("Debuff")]
    public Debuff debuff;
    public NumOfTargets numOfTargetsToDebuff;
    public int debuffDuration = 1;
    public bool debuffRandom = false;
    [Header("Heal")]
    public int heal;
    public NumOfTargets numOfTargetsToHeal;
    public bool healRandom = false;

    public virtual void Activate(GameObject caster, List<Damageable> enemyTargets, List<Damageable> allyTargets)
    {
        Debug.Log($"AKTIVIRA SE ABILITY {abilityName}");
        // Aktivacija Attacka
        if (type.HasFlag(AbilityType.Attack))
        {
            List<Damageable> selectedEnemyTargets = attackRandom ? PickTargets(enemyTargets) : enemyTargets;
            if (selectedEnemyTargets.Count == 0) return;
            // Logika za napad
            ExecuteAttack(selectedEnemyTargets);

            if(type.HasFlag(AbilityType.Debuff))
            {
                ExecuteDebuff(selectedEnemyTargets);
            }
            if (type.HasFlag(AbilityType.CrowdControl))
            {
                ExecuteCc(selectedEnemyTargets);
            }
        }
        // Aktivacija Debuffa
        if(type.HasFlag(AbilityType.Debuff) && !type.HasFlag(AbilityType.Attack))
        {
            List<Damageable> selectedEnemyTargets = debuffRandom ? PickTargets(enemyTargets) : enemyTargets;
            if (selectedEnemyTargets.Count == 0) return;

            ExecuteDebuff(selectedEnemyTargets);
        }
        // Aktivacija CC-a
        if (type.HasFlag(AbilityType.CrowdControl) && !type.HasFlag(AbilityType.Attack))
        {
            List<Damageable> selectedEnemyTargets = ccRandom ? PickTargets(enemyTargets) : enemyTargets;
            if (selectedEnemyTargets.Count == 0) return;

            ExecuteDebuff(selectedEnemyTargets);
        }
        // Aktivacija Defense-a
        if (type.HasFlag(AbilityType.Defense))
        {
            Debug.Log($"Postavlja defense!!! A BROJ TARGETA JE: {allyTargets.Count}");
            List<Damageable> selectedAllyTargets = defenseRandom ? PickTargets(allyTargets) : allyTargets;
            if (selectedAllyTargets.Count == 0) return;
            ExecuteDefense(selectedAllyTargets);

            if (type.HasFlag(AbilityType.Buff))
            {
                ExecuteBuff(selectedAllyTargets);
            }
        }
        // Aktivacija Heal-a
        if (type.HasFlag(AbilityType.Heal) && allyTargets.Count > 0)
        {
            List<Damageable> selectedAllyTargets = healRandom ? PickTargets(allyTargets) : allyTargets;
            if (selectedAllyTargets.Count == 0) return;
            ExecuteHeal(selectedAllyTargets);

            if (type.HasFlag(AbilityType.Buff))
            {
                ExecuteBuff(selectedAllyTargets);
            }
        }
        // Aktivacija Buff-a
        if(type.HasFlag(AbilityType.Buff) && !(type.HasFlag(AbilityType.Defense) || type.HasFlag(AbilityType.Heal)))
        {
            List<Damageable> selectedAllyTargets = buffRandom ? PickTargets(allyTargets) : allyTargets;
            if (selectedAllyTargets.Count == 0) return;
            ExecuteBuff(selectedAllyTargets);
        }
    }

    private void ExecuteAttack(List<Damageable> targets)
    {
        foreach(Damageable target in targets)
        {
            Debug.Log($"{target.name} is damaged with {attack} DMG.");
        }
    }

    private void ExecuteDefense(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} got {defense} shield.");
        }
    }

    private void ExecuteBuff(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is buffed with {buff}.");
        }
    }

    private void ExecuteDebuff(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is debuffed with {debuff}.");
        }
    }

    private void ExecuteCc(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is cc-ed with {crowdControlType}.");
        }
    }

    private void ExecuteHeal(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is healed with {attack} health.");
        }
    }

    // Random Picka Targete
    public virtual List<Damageable> PickTargets(List<Damageable> targets)
    {
        List<Damageable> selectedTargets = new();
        
        if(type == AbilityType.Attack)
        {
            int targetsToAttack = ConvertEnumToInt(numOfTargetsToAttack);
            selectedTargets = GetTargets(targets, targetsToAttack);
        }
        
        return selectedTargets;
    }

    private List<Damageable> GetTargets(List<Damageable> targets, int numOfTargets)
    {
        List<Damageable> copyOfTargets = new(targets);

        if(numOfTargets > copyOfTargets.Count)
        {
            return copyOfTargets;
        }

        List<Damageable> selectedTargets = new();

        for(int i = 0; i < numOfTargets; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, copyOfTargets.Count);
            selectedTargets.Add(copyOfTargets[randomIndex]);
            copyOfTargets.RemoveAt(randomIndex);
        }

        return selectedTargets;
    }

    private int ConvertEnumToInt(NumOfTargets not)
    {
        int enumToInt = 0;
        if (not == NumOfTargets.None)
        {
            enumToInt = 0;
        }
        else if (not == NumOfTargets.One)
        {
            enumToInt = 1;
        }
        else if (not == NumOfTargets.Two)
        {
            enumToInt = 2;
        }
        else if (not == NumOfTargets.Three)
        {
            enumToInt = 3;
        }
        else if (not == NumOfTargets.All)
        {
            enumToInt = 4;
        }

        return enumToInt;
    }

    public int AbilityNeedEnemyTargets()
    {
        int needTargets = 0;

        if (type.HasFlag(AbilityType.Attack) && !attackRandom) needTargets = ConvertEnumToInt(numOfTargetsToAttack);
        if (type.HasFlag(AbilityType.CrowdControl) && !ccRandom) needTargets = ConvertEnumToInt(numOfTargetsToCC);
        if (type.HasFlag(AbilityType.Debuff) && !debuffRandom) needTargets = ConvertEnumToInt(numOfTargetsToDebuff);

        return needTargets;
    }

    public int AbilityNeedAllyTargets()
    {
        int needTargets = 0;

        if (type.HasFlag(AbilityType.Defense) && !defenseRandom) needTargets = ConvertEnumToInt(numOfTargetsToDefense);
        if (type.HasFlag(AbilityType.Buff) && !buffRandom) needTargets = ConvertEnumToInt(numOfTargetsToBuff);
        if (type.HasFlag(AbilityType.Heal) && !healRandom) needTargets = ConvertEnumToInt(numOfTargetsToHeal);

        return needTargets;
    }
}
