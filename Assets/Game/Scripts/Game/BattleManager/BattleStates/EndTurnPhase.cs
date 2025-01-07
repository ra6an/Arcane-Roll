using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private BattleManager _battleManager;
    private EnemiesController _enemyController;
    private PlayerTeamController _playerTeamController;

    List<Damageable> enemyTeam;
    List<Damageable> allyTeam;

    public bool canChangeState { get; set; }

    public EndTurnPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _enemyController = GameManager.Instance.GetComponent<EnemiesController>();
        _playerTeamController = GameManager.Instance.GetComponent<PlayerTeamController>();
        canChangeState = false;
    }

    public void EnterState()
    {
        enemyTeam = _enemyController.GetAllEnemies();
        allyTeam = _playerTeamController.GetAllyMonstersDamageable();
    }

    public void UpdateState()
    {
        if(enemyTeam.Count == 0 || allyTeam.Count == 0)
        {
            // End Battle
            _battleManager.SetBattleIsOver();
        } else
        {
            _battleManager.SetNextTurn();
            _battleManager.ResetAditionalDiceRolls();
            _battleStateMachine.ChangeState<EnemyPlanningPhase>();
        }
    }

    public void ExitState()
    {
        Debug.Log("Izlaz iz endturna");
    }

    public void ChangeState()
    {
        Debug.Log("Mjenja state");
    }
}
