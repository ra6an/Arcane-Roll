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

    bool stateChanged = false;

    public bool canChangeState { get; set; }

    public EndTurnPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _enemyController = GameManager.Instance.GetComponent<EnemiesController>();
        _playerTeamController = GameManager.Instance.GetComponent<PlayerTeamController>();
    }

    public void EnterState()
    {
        canChangeState = false;
        enemyTeam = _enemyController.GetAllEnemies();
        allyTeam = _playerTeamController.GetAllyMonstersDamageable();
    }

    public void UpdateState()
    {
        if(!stateChanged)
        {
            _battleManager.SetNextTurn();
            _battleManager.ResetAditionalDiceRolls();
            HandleAllyTeamBuffsDebuffsAndShields();
            HandleEnemyTeamBuffsDebuffsAndShields();
            _battleStateMachine.ChangeState<EnemyPlanningPhase>();
            stateChanged = true;

            foreach(Damageable enemy in enemyTeam)
            {
                enemy.HandleEndTurnEffects();
            }
            foreach(Damageable ally in allyTeam)
            {
                ally.HandleEndTurnEffects();
            }
        }
    }

    public void ExitState()
    {
        stateChanged = false;
    }

    public void ChangeState()
    {
        Debug.Log("Mjenja state");
    }

    private void HandleAllyTeamBuffsDebuffsAndShields()
    {
        foreach(Damageable ally in allyTeam)
        {
            ally.ClearShield();
        }
    }
    private void HandleEnemyTeamBuffsDebuffsAndShields()
    {
        foreach(Damageable enemy in enemyTeam)
        {
            enemy.ClearShield();
        }
    }
}
