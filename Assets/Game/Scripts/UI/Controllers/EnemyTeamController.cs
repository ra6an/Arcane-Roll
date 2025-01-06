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
}
