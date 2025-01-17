using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    private GameObject enemySpawns;
    [SerializeField] private Vector3[] oneEnemyPosition;
    [SerializeField] private Vector3[] positions;
    [SerializeField] private Vector3[] twoEnemiesPositions;

    private void Awake()
    {
        enemySpawns = GameObject.FindWithTag("EnemySpawnPoints");
    }

    public void InstantiateEnemies(EnemyTeamSO et, BattleRoomSO bs)
    {
        if(et == null || bs  == null) return;
        ClearEnemies();

        enemySpawns.transform.localPosition = bs.EnemyPositions;
        int enemiesInTeam = ReturnNumOfEnemiesInTeam(et);

        int count = 0;
        foreach(EnemySO e in et.Enemies)
        {
            if(e != null && e.enemyPrefab != null)
            {
                GameObject ego = Instantiate(e.enemyPrefab, enemySpawns.transform);
                Vector3 pickedPoss = new();

                if(enemiesInTeam == 1)
                {
                    pickedPoss = oneEnemyPosition[count];
                } else if(enemiesInTeam == 2)
                {
                    pickedPoss = twoEnemiesPositions[count];
                } else if (enemiesInTeam == 3)
                {
                    pickedPoss = positions[count];
                }

                ego.transform.SetLocalPositionAndRotation(pickedPoss, bs.EnemyRotation);
                EnemyController enemyControllerCheck = ego.GetComponent<EnemyController>();

                if (enemyControllerCheck == null)
                {
                    ego.AddComponent<EnemyController>();
                    Debug.LogWarning("Enemy Prefab does not have EnemyController.cs Script!!!");
                }

                Effectable eff = ego.GetComponent<Effectable>();

                if(eff == null)
                {
                    ego.AddComponent<Effectable>();
                }
            
                EnemyController enemyController = ego.GetComponent<EnemyController>();
                enemyController.SetEnemyData(e, count);

                count++;
            }
        }
    }

    private int ReturnNumOfEnemiesInTeam(EnemyTeamSO et)
    {
        int enemiesInTeam = 0;

        foreach(EnemySO enemy in et.Enemies)
        {
            if(enemy == null) continue;
            enemiesInTeam++;
        }

        return enemiesInTeam;
    }

    private void ClearEnemies()
    {
        int childCount = enemySpawns.transform.childCount;
        if (childCount == 0) return;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = enemySpawns.transform.GetChild(i);
            DOTween.Kill(child.transform);
            Destroy(child.gameObject);
        }
    }

    public void ClearEnemiesPrefab()
    {
        ClearEnemies();
    }

    public void SetEnemiesAttacks()
    {
        foreach(EnemyController enemy in  enemySpawns.GetComponentsInChildren<EnemyController>())
        {
            bool isAlive = enemy.GetComponent<Damageable>().IsAlive();
            if(isAlive)
            {
                enemy.SetRandomNumber();
            }
        }
        foreach (EnemyController enemy in enemySpawns.GetComponentsInChildren<EnemyController>())
        {
            bool isAlive = enemy.GetComponent<Damageable>().IsAlive();
            if (isAlive)
            {
                enemy.ShowAbility();
            }
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
