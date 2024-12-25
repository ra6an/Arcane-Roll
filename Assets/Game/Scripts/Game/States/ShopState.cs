using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : IGameState
{
    private StateMachine _stateMachine;

    public string StateName => "ShopState";

    public ShopState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState() { }

    public void UpdateState() { }

    public void ExitState() { }
}
