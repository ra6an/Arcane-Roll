using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IGameState
{
    private StateMachine _stateMachine;

    public string StateName => "GameOverState";

    public GameOverState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState() { }

    public void UpdateState() { }

    public void ExitState() { }
}
