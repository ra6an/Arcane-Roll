using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    None,
    Attack,
    Defense,
    CrowdControl,
    Buff,
    Debuff,
    Heal,
    Multiply,
}

public enum NumOfEnemies
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
    public NumOfEnemies attackEnemies;
    public int numOfAttacks;
    public bool lifesteal = false;
    public bool execute = false;
    [Header("Defense")]
    public int defense;
    [Header("Crowd Control")]
    public CC crowdControl;
    public NumOfEnemies numOfCcEnemies;
    public int crowdControlDuration;
    [Header("Buff")]
    public Buff buff;
    [Header("Debuff")]
    public Debuff debuff;
    public NumOfEnemies numOfDebufEnemies;
    public int debuffDuration;

    // Event-driven effecti

    public virtual void Activate(GameObject monster, GameObject[] targets)
    {
        if(type == AbilityType.Attack)
        {
            // Logika za napar
            Debug.Log($"Attack for: {attack}");
        }


    }
}
