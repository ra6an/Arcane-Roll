using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private EnemyTeamSO enemyTeam;
    private int initialDiceRolls = 1;
    private int additionalDiceRolls = 0;
    private int currentAmountOfRolls = 0;
    private int currentTurn;
    private bool battleIsOver = false;

    public EnemyTeamSO EnemyTeam => enemyTeam;
    public int CurrentTurn => currentTurn;
    public bool BattleIsOver => battleIsOver;
    public int CurrentAmountOfRolls => currentAmountOfRolls;

    private BattleStateMachine _battleStateMachine;

    private void Start()
    {
        if(BattleStateMachine.Instance == null)
        {
            _battleStateMachine = new BattleStateMachine();
        } else
        {
            _battleStateMachine = BattleStateMachine.Instance;
        }

        _battleStateMachine.Initialize();
        // Subscribe na event u bsm
        _battleStateMachine.OnBattleStateChanged += HandleBattleStateChanged;
    }

    private void Update()
    {
        if (_battleStateMachine == null) return;
        _battleStateMachine.Update();
    }

    private void HandleBattleStateChanged(Type newState)
    {
        if(newState == typeof(PlayerPreparationPhase))
        {
            RelicsManager.Instance.OnPlayerPreparationPhaseStarted();
        }
    }

    public void SetEnemyTeam(EnemyTeamSO et)
    {
        if(et != null)
        {
            enemyTeam = et;
        }
    }

    private void ResetBattleState()
    {
        currentTurn = 0;
        additionalDiceRolls = 0;
        currentAmountOfRolls = 0;
        battleIsOver = false;
    }

    internal void SetupBattleState(EnemyTeamSO et)
    {
        ResetBattleState();
        enemyTeam = et;
        _battleStateMachine.ChangeState<EnemyPlanningPhase>();
    }

    public IBattleState GetState()
    {
        IBattleState currState = _battleStateMachine.CurrentBattleState;

        if (currState == null) return null;

        return currState;
    }

    public void ResetAditionalDiceRolls()
    {
        additionalDiceRolls = 0;
    }

    public void AddMoreDiceRolls(int _extraRolls)
    {
        additionalDiceRolls += _extraRolls;
    }

    public void SetCurrentAmountOfRolls()
    {
        currentAmountOfRolls = initialDiceRolls + additionalDiceRolls;
    }

    public void RemoveOneRoll()
    {
        currentAmountOfRolls--;
    }

    public void SetBattleIsOver()
    {
        battleIsOver = true;
    }

    public void SetNextTurn()
    {
        currentTurn++;
    }

    private void OnDestroy()
    {
        // Unsubscribe od evenata na bsm-u
        _battleStateMachine.OnBattleStateChanged -= HandleBattleStateChanged;
    }
}
