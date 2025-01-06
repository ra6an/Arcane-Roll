using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    private GameObject enemySpawns;
    [SerializeField] private Vector3[] positions;

    private void Awake()
    {
        enemySpawns = GameObject.FindWithTag("EnemySpawnPoints");
    }

    public void InstantiateEnemies(EnemyTeamSO et, BattleRoomSO bs)
    {
        if(et == null || bs  == null) return;

        enemySpawns.transform.localPosition = bs.EnemyPositions;

        int count = 0;
        foreach(EnemySO e in et.Enemies)
        {
            if(e != null && e.enemyPrefab != null)
            {
                GameObject ego = Instantiate(e.enemyPrefab, enemySpawns.transform);

                ego.transform.SetLocalPositionAndRotation(positions[count], bs.EnemyRotation);
                EnemyController enemyControllerCheck = ego.GetComponent<EnemyController>();

                if (enemyControllerCheck == null)
                {
                    ego.AddComponent<EnemyController>();
                    Debug.LogWarning("Enemy Prefab does not have EnemyController.cs Script!!!");
                }
            
                EnemyController enemyController = ego.GetComponent<EnemyController>();
                enemyController.SetEnemyData(e, count);

                count++;
            }
        }
    }

    public void SetEnemiesAttacks()
    {
        foreach(EnemyController enemy in  enemySpawns.GetComponentsInChildren<EnemyController>())
        {
            enemy.SetRandomNumber();
        }
        foreach (EnemyController enemy in enemySpawns.GetComponentsInChildren<EnemyController>())
        {
            enemy.ShowAbility();
        }
    }

    public bool CheckIfEnemiesAreReady()
    {
        bool enemyTeamIsReady = true;

        foreach(EnemyController enemy in enemySpawns.GetComponentsInChildren<EnemyController>() )
        {
            if(enemy.RolledNumber == -1)
            {
                enemyTeamIsReady = false;
            }
        }
        //Debug.Log(enemyTeamIsReady);
        return enemyTeamIsReady;
    }

    public void HideEnemiesAbilitieDetails()
    {
        foreach (EnemyController enemy in enemySpawns.GetComponentsInChildren<EnemyController>())
        {
            enemy.HideAbility();
        }
    }

    public List<Damageable> GetAllEnemies()
    {
        List<Damageable> aliveEnemies = new();

        foreach(Transform enemy in enemySpawns.transform)
        {
            if (enemy == null) continue;

            Damageable ed = enemy.GetComponent<Damageable>();

            if(ed == null) continue;

            if(ed.IsAlive())
            {
                aliveEnemies.Add(ed);
            }
        }

        return aliveEnemies;
    }

    public Damageable GetEnemyDamageableBySpawnId(int _spawnId)
    {
        Damageable selectedEnemy = null;

        foreach(Transform enemy in enemySpawns.transform)
        {
            EnemyController ecEnemy = enemy.GetComponent<EnemyController>();
            if(ecEnemy.SpawnId == _spawnId)
            {
                selectedEnemy = enemy.GetComponent<Damageable>();
                break;
            }
        }

        return selectedEnemy;
    }
}
