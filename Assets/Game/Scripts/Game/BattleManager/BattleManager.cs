using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum BattlePhase
//{
//    EnemyPreparation,
//    PlayerPreparation,
//    PlayerBattle,
//    EnemyBattle,
//    EndTurn
//}

public class BattleManager : MonoBehaviour
{
    private EnemyTeamSO enemyTeam;
    private int currentTurn;
    private bool battleIsOver = false;

    public EnemyTeamSO EnemyTeam => enemyTeam;
    public int CurrentTurn => currentTurn;
    public bool BattleIsOver => battleIsOver;

    private BattleStateMachine _battleStateMachine;

    private void Start()
    {
        _battleStateMachine = new BattleStateMachine();
    }

    private void Update()
    {
        _battleStateMachine.Update();
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
        battleIsOver = false;
    }

    internal void SetupBattleState(EnemyTeamSO et)
    {
        ResetBattleState();
        enemyTeam = et;
        _battleStateMachine = new BattleStateMachine();
        _battleStateMachine.StartState();
    }

    public IBattleState GetState()
    {
        IBattleState currState = _battleStateMachine.CurrentBattleState;

        if (currState == null) return null;

        return currState;
    }
}
