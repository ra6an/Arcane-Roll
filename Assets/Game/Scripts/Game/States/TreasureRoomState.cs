using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomState : IGameState
{
    private StateMachine _stateMachine;

    public string StateName => "TreasureRoomState";

    public TreasureRoomState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState() { }

    public void UpdateState() { }

    public void ExitState() { }
}
