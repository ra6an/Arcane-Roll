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
        throw new System.NotImplementedException();
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeState()
    {

    }
}
