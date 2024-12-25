using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Buff Ability")]
public class BuffAbilitySO : AbilitySO
{
    public int buffDuration; // Turns
    
    public override void Activate(GameObject monster, GameObject[] targets)
    {
        base.Activate(monster, targets);
        Debug.Log($"{monster.name} applies buff to {targets[0].name} for {buffDuration} turns.");

        // Dodati logiku za buffove
    }
}
