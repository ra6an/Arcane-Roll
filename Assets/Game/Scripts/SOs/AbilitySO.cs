using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityExecutionContext
{
    public int amount;
    public bool lethal;
    public bool piercing;
    public bool lifesteal;
    public bool self;

    public AbilityExecutionContext()
    {
        lethal = false;
        piercing = false;
        lifesteal = false;
        self = false;
    }
}

[Flags]
public enum AbilityType
{
    None = 0,
    Attack = 1 << 0,
    Defense = 1 << 1,
    CrowdControl = 1 << 2,
    Buff = 1 << 3,
    Debuff = 1 << 4,
    Status = 1 << 5,
    Heal = 1 << 6,
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

[Flags]
public enum Buff
{
    None = 0,
    Strength = 1 << 0,
    Vitality = 1 << 1,
    Immortal = 1 << 2,
    Unstoppable = 1 << 3,
    Piercing = 1 << 4,
    Divinity = 1 << 5,
}

[Flags]
public enum Debuff
{
    None = 0,
    Vulnerable = 1 << 0,
    Weak = 1 << 1,
    Shatter = 1 << 2,
    Depressed = 1 << 3,
}

[Flags]
public enum Status
{
    None = 0,
    Burn = 1 << 0,
    Poison = 1 << 1,
    Chill = 1 << 2,
}

[CreateAssetMenu(menuName = "Data/Ability")]
public class AbilitySO : ScriptableObject, IAbility
{
    public int id;
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
    public bool piercing = false;
    public bool attackRandom = false;
    public bool applyEffectToTarget = false;
    [Header("Defense")]
    public int defense;
    public NumOfTargets numOfTargetsToDefense;
    public bool lethalDefenseBoost = false;
    public bool defenseRandom = false;
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
    public bool applyDebuffOnSameTarget = false;
    [Header("Debuff")]
    public Debuff debuff;
    public NumOfTargets numOfTargetsToDebuff;
    public int debuffDuration = 1;
    public bool debuffRandom = false;
    [Header("Status")]
    public Status status;
    public NumOfTargets numOfTargetsToStatus;
    public float chanceToApplyStatus = 1f;
    public int statusDuration = 1;
    public bool statusRandom = false;
    [Header("Heal")]
    public int heal;
    public NumOfTargets numOfTargetsToHeal;
    public bool lethalHealBoost = false;
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
            ExecuteAttack(caster, selectedEnemyTargets);
            if(lifesteal)
            {
                caster.GetComponent<Damageable>().Lifesteal(attack);
            }

            if(type.HasFlag(AbilityType.Debuff))
            {
                ExecuteDebuff(selectedEnemyTargets);
            }
            if (type.HasFlag(AbilityType.CrowdControl))
            {
                ExecuteCc(selectedEnemyTargets);
            }

            if (type.HasFlag(AbilityType.Status) && applyEffectToTarget)
            {
                ExecuteStatus(selectedEnemyTargets);
            }
        }
        // Aktivacija Debuffa
        if(type.HasFlag(AbilityType.Debuff) && !type.HasFlag(AbilityType.Attack) && !applyDebuffOnSameTarget)
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

            ExecuteCc(selectedEnemyTargets);
        }
        // Aktivacija Defense-a
        if (type.HasFlag(AbilityType.Defense))
        {
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
        if(type.HasFlag(AbilityType.Buff) && !(type.HasFlag(AbilityType.Defense) || type.HasFlag(AbilityType.Heal) || type.HasFlag(AbilityType.Attack)))
        {
            List<Damageable> selectedAllyTargets = buffRandom ? PickTargets(allyTargets) : allyTargets;
            if (selectedAllyTargets.Count == 0) return;
            ExecuteBuff(selectedAllyTargets);

            if(type.HasFlag(AbilityType.Debuff) && applyDebuffOnSameTarget)
            {
                ExecuteDebuff(selectedAllyTargets);
            }
        }
        // Aktivacija Status Effect-a
        if(type.HasFlag(AbilityType.Status) && !(type.HasFlag(AbilityType.Attack)))
        {
            List<Damageable> selectedEnemyTargets = statusRandom ? PickTargets(enemyTargets) : enemyTargets;
            if (selectedEnemyTargets.Count == 0) return;
            ExecuteStatus(selectedEnemyTargets);
        }
    }

    private void ExecuteAttack(GameObject _caster, List<Damageable> targets)
    {
        Damageable dmg = _caster.GetComponent<Damageable>();

        if (dmg == null) return;

        int modifiedAttackValue = dmg.ModifyAttack(attack);
        Debug.Log($"Casters modified attack: {modifiedAttackValue}.");

        foreach (Damageable target in targets)
        {
            AbilityExecutionContext aec = new()
            {
                amount = modifiedAttackValue,
                lethal = execute,
                piercing = piercing
            };

            target.TakeDamage(aec);
        }
    }

    private void ExecuteDefense(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            AbilityExecutionContext aec = new()
            {
                amount = defense,
                lethal = lethalDefenseBoost,
            };

            target.AddShield(aec);
        }
    }

    private void ExecuteBuff(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is buffed with {buff}.");
            EffectBase _buff = null;

            if(buff == Buff.Strength)
            {
                _buff = new Strength();
            }
            if(buff == Buff.Vitality)
            {
                _buff = new Vitality();
            }

            if(_buff != null)
            {
                target.AddEffect(_buff, buffDuration);
            }
        }
    }

    private void ExecuteDebuff(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is debuffed with {debuff}.");
            EffectBase _debuff = null;
            int _duration = debuffDuration;

            if(debuff == Debuff.Vulnerable)
            {
                _debuff = new Vulnerable();
            }
            if(debuff == Debuff.Weak)
            {
                _debuff = new Weak();
            }

            if(_debuff != null)
            {
                target.AddEffect(_debuff, _duration);
            }
        }
    }

    private void ExecuteCc(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            Debug.Log($"{target.name} is cc-ed with {crowdControlType}.");
        }
    }

    private void ExecuteStatus(List<Damageable> targets)
    {
        EffectBase _status = null;
        int _duration = statusDuration;

        // Postavljamo Status Effekat
        if (status.HasFlag(Status.Burn))
        { 
            _status = new Burn();
        }
        if(status.HasFlag(Status.Poison))
        {
            _status = new Poison();
        }
        if(status.HasFlag(Status.Chill))
        {
            _status = new Chill();
        }

        if (_status == null) return;

        foreach (Damageable target in targets)
        {
            Debug.Log($"Applying Status: {status} for {statusDuration} turns on -- {target.gameObject.name} --!");
            System.Random rnd = new();
            float rndNum = (float)rnd.NextDouble();

            if (rndNum <= chanceToApplyStatus)
            {
                target.AddEffect(_status, _duration);
            }
        }
    }

    private void ExecuteHeal(List<Damageable> targets)
    {
        foreach (Damageable target in targets)
        {
            AbilityExecutionContext aec = new()
            {
                amount = heal,
                lethal = lethalHealBoost
            };

            target.Heal(aec);
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

        if(type.HasFlag(AbilityType.Defense))
        {
            int targetsToDefense = ConvertEnumToInt(numOfTargetsToDefense);
            selectedTargets = GetTargets(targets, targetsToDefense);
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
        if (type.HasFlag(AbilityType.Debuff) && !debuffRandom && !applyDebuffOnSameTarget) needTargets = ConvertEnumToInt(numOfTargetsToDebuff);

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
