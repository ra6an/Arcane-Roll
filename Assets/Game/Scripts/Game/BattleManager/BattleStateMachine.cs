using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStateMachine
{
    public IBattleState CurrentBattleState { get; private set; }

    private Dictionary<Type, IBattleState> _battleStates;
    
    private void Start()
    {
        _battleStates = new Dictionary<Type, IBattleState>
        {
            { typeof(EnemyPlanningPhase), new EnemyPlanningPhase(this) },
            { typeof(PlayerPreparationPhase), new PlayerPreparationPhase(this) },
            { typeof(PlayerBattlePhase), new PlayerBattlePhase(this) },
            { typeof(EnemyBattlePhase), new EnemyBattlePhase(this) },
            { typeof(EndTurnPhase), new EndTurnPhase(this) },
        };
    }

public void SetState(IBattleState newState)
    {
        CurrentBattleState?.ExitState();
        CurrentBattleState = newState;
        CurrentBattleState.EnterState();
    }

    public void Update()
    {
        CurrentBattleState?.UpdateState();
    }

    public void ChangeState<T>() where T : IGameState
    {
        if (CurrentBattleState != null)
        {
            CurrentBattleState.ExitState();
        }
        Debug.Log($"Changing state from: {CurrentBattleState} to: {typeof(T).Name}");

        CurrentBattleState = _battleStates[typeof(T)];
        CurrentBattleState.EnterState();
        Debug.Log($"State changed to: {CurrentBattleState}");
    }
}
