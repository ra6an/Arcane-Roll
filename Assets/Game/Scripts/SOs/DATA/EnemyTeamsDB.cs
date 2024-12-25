using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Enemy Teams")]
public class EnemyTeamsDB : ScriptableObject
{
    [SerializeField] private List<EnemyTeamSO> enemyTeams;

    public List<EnemyTeamSO> EnemyTeams => enemyTeams;
}
