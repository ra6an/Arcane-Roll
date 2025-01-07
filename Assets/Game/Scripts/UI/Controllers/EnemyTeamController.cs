using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamController : MonoBehaviour
{
    private EnemyTeamSO enemyTeam;
    [SerializeField] private GameObject enemyTeamContainer;
    [SerializeField] private GameObject enemyDetailsPrefab;

    public void SetEnemyTeamDetails(EnemyTeamSO _enemyTeam)
    {
        if (_enemyTeam == null) return;

        enemyTeam = _enemyTeam;

        int count = 0;
        foreach (EnemySO enemy in enemyTeam.Enemies)
        {
            GameObject go = Instantiate(enemyDetailsPrefab, enemyTeamContainer.transform);
            go.GetComponent<EnemyDetails>().SetEnemyDetails(enemy, count);
            count++;
        }
    }

    public GameObject SetEnemyDetails(EnemySO e, int _spawnId)
    {
        if(e == null || e.enemyName == null) return null;

        GameObject go = Instantiate(enemyDetailsPrefab, enemyTeamContainer.transform);
        go.GetComponent<EnemyDetails>().SetEnemyDetails(e, _spawnId);
        return go;
    }

    public void ShowEnemiesCheckBoxes()
    {
        foreach (GameObject e in transform)
        {
            EnemyDetails eDetails = e.GetComponent<EnemyDetails>();
            if(eDetails != null)
            {
                eDetails.ShowCheckBox();
            }
        }
    }

    public List<EnemyDetails> GetEnemyDetails()
    {
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        List<EnemyDetails> enemies = new();

        foreach(Transform e in enemyTeamContainer.transform)
        {
            EnemyDetails eDetails = e.GetComponent<EnemyDetails>();
            Damageable eDamageable = ec.GetEnemyDamageableBySpawnId(eDetails.SpawnId);

            if(eDamageable.IsAlive())
            {
                enemies.Add(eDetails);
            }
        }

        return enemies;
    }

    public List<EnemyDetails> GetEnemyDetailsWithActiveAbility()
    {
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        List<EnemyDetails> enemies = new();

        foreach (Transform e in enemyTeamContainer.transform)
        {
            EnemyDetails eDetails = e.GetComponent<EnemyDetails>();
            Damageable eDamageable = ec.GetEnemyDamageableBySpawnId(eDetails.SpawnId);
            Debug.Log($"{e.name}: IS ALIVE - {eDamageable.IsAlive()} === have ability - {eDetails.ActiveAbility}");
            if (eDamageable.IsAlive() && eDetails.ActiveAbility != null)
            {
                enemies.Add(eDetails);
            }
        }

        return enemies;
    }

    public void ClearContainer()
    {
        int childCount = enemyTeamContainer.transform.childCount;
        if (childCount == 0) return;

        Debug.Log($"Brisemo enemy monstere u UI - trenutno ima {childCount}");
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = enemyTeamContainer.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        Debug.Log($"Enemy monstera nakon brisanja: {enemyTeamContainer.transform.childCount}");
        //if (enemyTeamContainer.transform.childCount == 0) return;

        //foreach(Transform child in enemyTeamContainer.transform)
        //{
        //    Destroy(child.gameObject);
        //}
    }
}
