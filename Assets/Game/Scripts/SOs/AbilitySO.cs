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
    public NumOfEnemies numOfTargetsToAttack;
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

    public virtual List<Damageable> PickTargets(List<Damageable> targets)
    {
        List<Damageable> selectedTargets = new();
        //Debug.Log($"NumOfTargets: {targets.Count}");
        if(type == AbilityType.Attack)
        {
            int targetsToAttack = 0;

            if(numOfTargetsToAttack == NumOfEnemies.None)
            {
                targetsToAttack = 0;
            } else if (numOfTargetsToAttack == NumOfEnemies.One) {
                targetsToAttack = 1;
            } else if (numOfTargetsToAttack == NumOfEnemies.Two)
            {
                targetsToAttack = 2;
            } else if (numOfTargetsToAttack == NumOfEnemies.Three)
            {
                targetsToAttack = 3;
            } else if (numOfTargetsToAttack == NumOfEnemies.All)
            {
                targetsToAttack = 4;
            }

            selectedTargets = GetTargets(targets, targetsToAttack);
        }
        //Debug.Log($"NumOf Selected Targets: {selectedTargets}");
        return selectedTargets;
    }

    public virtual void Activate(GameObject monster, GameObject[] targets)
    {
        if(type == AbilityType.Attack)
        {
            // Logika za napar
            Debug.Log($"Attack for: {attack}");
        }
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
            int randomIndex = Random.Range(0, copyOfTargets.Count);
            selectedTargets.Add(copyOfTargets[randomIndex]);
            copyOfTargets.RemoveAt(randomIndex);
        }

        return selectedTargets;
    }
}
