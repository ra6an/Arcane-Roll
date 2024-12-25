using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IGameState
{
    private StateMachine _stateMachine;

    private EnemyTeamSO enemyTeam;

    public string StateName => "BattleState";
    public EnemyTeamSO EnemyTeam => enemyTeam;

    public BattleState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        CameraController _mainCamera = Camera.main.GetComponent<CameraController>();
        _mainCamera.MoveCameraToTarget(ExitStateSetup);
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        cc.ShowBattleUI();

        if(enemyTeam != null)
        {
            cc.enemyTeamPanel.GetComponent<EnemyTeamController>().SetEnemyTeamDetails(enemyTeam);
        }
    }

    public void UpdateState() { }

    public void ExitState() { }

    private void ExitStateSetup()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        cc.ShowBattleUI();
    }

    public void SetEnemyTeam(EnemyTeamSO _et)
    {
        enemyTeam = _et;
    }
}
