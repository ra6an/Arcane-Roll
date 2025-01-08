using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IGameState
{
    private StateMachine _stateMachine;

    private EnemyTeamSO enemyTeam;
    private BattleRoomSO battleRoom;

    public string StateName => "BattleState";
    public EnemyTeamSO EnemyTeam => enemyTeam;
    public BattleRoomSO BattleRoom => battleRoom;

    public BattleState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        GameManager.Instance.GetComponent<EnemiesController>().InstantiateEnemies(enemyTeam, battleRoom);
        
        PlayerController.Instance.MovePlayerOnBattleStart(battleRoom, ExitStateSetup);
        
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        cc.ShowBattleUI();

        if(enemyTeam != null)
        {
            GameManager.Instance.GetComponent<BattleManager>().SetupBattleState(enemyTeam);
        }
    }

    public void UpdateState()
    {
        if(CheckIfBattleEnded())
        {
            _stateMachine.ChangeState<MapState>();
        }
    }

    public void ExitState()
    {
        ClearAllBattlePrefabs();
    }

    private void ClearAllBattlePrefabs()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        PlayerTeamController ptc = GameManager.Instance.GetComponent<PlayerTeamController>();
        PlayerMonstersController pmc = cc.playerMonstersPanel.GetComponent<PlayerMonstersController>();
        EnemyTeamController etc = cc.enemyTeamPanel.GetComponent<EnemyTeamController>();

        ec.ClearEnemiesPrefab();
        ptc.ClearCrystalsPrefabs();
        pmc.ClearContainer();
        etc.ClearContainer();
        DiceManager.Instance.ClearDiceRollStates();
    }

    private void ExitStateSetup()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        cc.ShowBattleUI();
        GameManager.Instance.GetComponent<PlayerTeamController>().MaterializeCrystals();
    }

    public void SetEnemyTeam(EnemyTeamSO _et, BattleRoomSO _br)
    {
        enemyTeam = _et;
        battleRoom = _br;
    }

    private bool CheckIfBattleEnded()
    {
        return GameManager.Instance.GetComponent<BattleManager>().BattleIsOver;
    }
}
