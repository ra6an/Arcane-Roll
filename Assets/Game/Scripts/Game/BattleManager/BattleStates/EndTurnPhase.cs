using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;

    public EndTurnPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
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
}
