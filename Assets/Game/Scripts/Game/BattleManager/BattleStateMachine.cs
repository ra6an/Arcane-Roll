using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStateMachine 
{
    public static BattleStateMachine Instance { get; private set; }
    public IBattleState CurrentBattleState { get; private set; }

    private Dictionary<Type, IBattleState> _battleStates;

    // EVENTS
    public event Action<Type> OnBattleStateChanged = delegate { };

    public BattleStateMachine()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("Multiple instances of BattleStateMachine detected!");
        }
    }

    public void Initialize()
    {
        Debug.Log("INICIJALIZACIJA!!!!!!!!!!!!");
        _battleStates = new Dictionary<Type, IBattleState>
        {
            { typeof(EnemyPlanningPhase), new EnemyPlanningPhase(this) },
            { typeof(PlayerPreparationPhase), new PlayerPreparationPhase(this) },
            { typeof(PlayerBattlePhase), new PlayerBattlePhase(this) },
            { typeof(EnemyBattlePhase), new EnemyBattlePhase(this) },
            { typeof(EndTurnPhase), new EndTurnPhase(this) },
        };
    }

    public void Update()
    {
        if (CurrentBattleState == null) return;
        CurrentBattleState?.UpdateState();
    }

    public void ChangeState<T>() where T : IBattleState
    {
        CurrentBattleState?.ExitState();

        OnBattleStateChanged.Invoke(typeof(T));

        CurrentBattleState = _battleStates[typeof(T)];
        CurrentBattleState.EnterState();
    }
}
