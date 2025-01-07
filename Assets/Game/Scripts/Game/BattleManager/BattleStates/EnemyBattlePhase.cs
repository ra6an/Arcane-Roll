using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattlePhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private EnemiesController _enemyController;
    private CanvasController _canvasController;
    private InfoPanelController _infoPanelController;
    private EnemyTeamController _enemyTeamController;
    private List<EnemyDetails> enemies = new();
    private List<EnemyDetails> enemiesThatTriggeredAbility = new();
    private bool processingActivatedAbility = false;

    public bool canChangeState { get; set; }

    public EnemyBattlePhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _enemyController = GameManager.Instance.GetComponent<EnemiesController>();
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        if( _canvasController != null )
        {
            _infoPanelController = _canvasController.infoPanel.GetComponent<InfoPanelController>();
            _enemyTeamController = _canvasController.enemyTeamPanel.GetComponent<EnemyTeamController>();
        }
    }

    public void EnterState()
    {
        Debug.Log("U enemy battle phaseu smo");
        enemies = _enemyTeamController.GetEnemyDetailsWithActiveAbility();
        canChangeState = false;
    }

    public void UpdateState()
    {
        if (canChangeState) return;
        // Dodati logiku za dozvolu prelaska u end phase
        if(enemies.Count == 0)
        {
            canChangeState = true;
            return;
        }

        if(!processingActivatedAbility)
        {
            MonsterAbilityTrigger();
        }
    }

    public void ExitState()
    {
        Debug.Log("Izlazak iz enemy battle phasea!");
        enemies.Clear();
        enemiesThatTriggeredAbility.Clear();
    }

    public void ChangeState()
    {
        _battleStateMachine.ChangeState<EndTurnPhase>();
    }

    private void MonsterAbilityTrigger()
    {
        FilterAliveEnemies();
        EnemyDetails e = enemies[0];
        Debug.Log(enemies.Count);
        if (e != null)
        {
            processingActivatedAbility = true;
            enemiesThatTriggeredAbility.Add(e);
            GameManager.Instance.StartCoroutine(PrepareAndActivateAbility(e));
        }
    }

    private void FilterAliveEnemies()
    {
        List<EnemyDetails> filtered = new();
        foreach(EnemyDetails ed in enemies)
        {
            if(ed.IsAlive() && ed.ActiveAbility != null) filtered.Add(ed);
        }
        enemies = filtered;
    }

    private IEnumerator PrepareAndActivateAbility(EnemyDetails e)
    {
        InfoMessageContext imc = new();
        imc.text = $"Enemy {e.EnemyData.enemyName} activate ability {e.ActiveAbility.abilityName.ToUpper()}";
        _infoPanelController.AddNewLog(imc);

        yield return new WaitForSeconds(2);

        e.ActivateAbility();
    }

    public void AbilityFinished()
    {
        FilterAliveEnemiesThatDidNotTriggerAbility();
        processingActivatedAbility = false;
    }

    private void FilterAliveEnemiesThatDidNotTriggerAbility()
    {
        List<EnemyDetails> filteredEnemies = new();
        List<EnemyDetails> aliveEnemies = _enemyTeamController.GetEnemyDetailsWithActiveAbility();

        foreach(EnemyDetails e in aliveEnemies)
        {
            if (enemiesThatTriggeredAbility.Contains(e)) continue;
            filteredEnemies.Add(e);
        }

        enemies = filteredEnemies;
    }
}
