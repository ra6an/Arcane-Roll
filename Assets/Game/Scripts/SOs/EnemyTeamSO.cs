using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDifficulty
{
    EASY,
    MEDIUM,
    HARD
}

[CreateAssetMenu(menuName = "Data/EnemyTeam")]
public class EnemyTeamSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private EnemySO[] enemies = new EnemySO[5];
    [SerializeField] private TeamDifficulty difficulty;

    private int maxEnemies = 3;

    public int Id => id;
    public EnemySO[] Enemies => enemies;
    public TeamDifficulty Difficulty => difficulty;

    private void OnValidate()
    {
        if(enemies.Length > maxEnemies)
        {
            EnemySO[] trimmedEnemies = new EnemySO[maxEnemies];

            for (int i = 0; i < maxEnemies; i++)
            {
                trimmedEnemies[i] = enemies[i];
            }

            enemies = trimmedEnemies;
        }
    }
}
