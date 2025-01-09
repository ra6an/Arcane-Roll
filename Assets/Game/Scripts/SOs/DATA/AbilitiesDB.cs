using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Abilities")]
public class AbilitiesDB : ScriptableObject
{
    [SerializeField] private List<AbilitySO> allyAbilities;
    [SerializeField] private List<AbilitySO> enemyAbilities;

    public List<AbilitySO> AllyAbilities => allyAbilities;
    public List<AbilitySO> EnemyAbilities => enemyAbilities;

    private void OnEnable()
    {
        GenerateAllyAbilityIDs();
        GenerateEnemyAbilityIDs();
    }

    private void GenerateAllyAbilityIDs()
    {
        for (int i = 0; i < allyAbilities.Count; i++)
        {
            allyAbilities[i].id = i;
        }
    }

    private void GenerateEnemyAbilityIDs()
    {
        for (int i = 0; i < enemyAbilities.Count; i++)
        {
            enemyAbilities[i].id = i;
        }
    }
}
