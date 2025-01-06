using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int spawnId;
    private CanvasController canvasController;
    private EnemySO enemyData;
    //private int currentHealth;
    private int rolledNumber;
    private GameObject enemyUIDetails;

    public int SpawnId => spawnId;
    public EnemySO EnemyData => enemyData;
    //public int CurrentHealth => currentHealth;
    public int RolledNumber => rolledNumber;

    private void Awake()
    {
        rolledNumber = -1;
        canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
    }

    public void SetEnemyData(EnemySO ed, int _spawnId)
    {
        if(ed != null)
        {
            spawnId = _spawnId;
            enemyData = ed;
            //currentHealth = ed.health;
            transform.GetComponent<Damageable>().SetCurrentHealth(ed.health);

            if(canvasController != null)
            {
                //SET UI DETAILS AND SAVE REFFERENCE FOR FUTURE EDITS
                GameObject go = canvasController.enemyTeamPanel.GetComponent<EnemyTeamController>().SetEnemyDetails(ed, _spawnId);
                enemyUIDetails = go;
            }
        }
    }

    public bool EnemyIsAlive()
    {
        return transform.GetComponent<Damageable>().GetHealth() > 0;
    }

    public void SetRandomNumber()
    {
        rolledNumber = Random.Range(0, enemyData.Abilities.Length);
        
        PlayerTeamController ptc = GameManager.Instance.GetComponent<PlayerTeamController>();
        List<Damageable> targetsToAttack = enemyData.Abilities[rolledNumber].PickTargets(ptc.GetAllyMonstersDamageable());
        
        enemyUIDetails.GetComponent<EnemyDetails>().SetAbilityDetails(rolledNumber, targetsToAttack);
    }

    public void ShowAbility()
    {
        enemyUIDetails.GetComponent<EnemyDetails>().ShowAbilityDescription();
    }

    public void HideAbility()
    {
        enemyUIDetails.GetComponent<EnemyDetails>().HideAbilityDescription();
    }
}
