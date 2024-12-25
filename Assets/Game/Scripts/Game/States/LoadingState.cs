using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingState : IGameState
{
    private StateMachine _stateMachine;
    bool changeState;

    public string StateName => "LoadingState";

    public LoadingState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();

        if(cc != null)
        {
            cc.ShowLoadingPanel();
            changeState = false;
            _stateMachine.StartCoroutine(ChangeToSetSelectionState());
        }
    }

    public void UpdateState()
    {
        if(changeState)
        {
            _stateMachine.ChangeState<SetSelectionState>();
        }
    }

    public void ExitState()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();

        if (cc != null)
        {
            cc.HideLoadingPanel();
        }
    }

    private IEnumerator ChangeToSetSelectionState()
    {
        yield return new WaitForSeconds(3f);
        changeState = true;
    }
}
