using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Activate(GameObject caster, List<Damageable> enemyTargets, List<Damageable> allyTargets);
}
