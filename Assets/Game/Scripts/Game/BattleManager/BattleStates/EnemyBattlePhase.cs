using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattlePhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;

    public bool canChangeState { get; set; }

    public EnemyBattlePhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        canChangeState = false;
    }

    public void EnterState()
    {
        Debug.Log("U enemy battle phaseu smo");
    }

    public void UpdateState()
    {
        // Dodati logiku za dozvolu prelaska u end phase
    }

    public void ExitState()
    {
        
    }

    public void ChangeState()
    {
        _battleStateMachine.ChangeState<EndTurnPhase>();
    }
}
