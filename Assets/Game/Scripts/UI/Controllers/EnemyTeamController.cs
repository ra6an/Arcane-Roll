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

        foreach (EnemySO enemy in enemyTeam.Enemies)
        {
            GameObject go = Instantiate(enemyDetailsPrefab, enemyTeamContainer.transform);
            go.GetComponent<EnemyDetails>().SetEnemyDetails(enemy);
        }
    }

    public GameObject SetEnemyDetails(EnemySO e)
    {
        if(e == null || e.enemyName == null) return null;

        GameObject go = Instantiate(enemyDetailsPrefab, enemyTeamContainer.transform);
        go.GetComponent<EnemyDetails>().SetEnemyDetails(e);
        return go;
    }
}
