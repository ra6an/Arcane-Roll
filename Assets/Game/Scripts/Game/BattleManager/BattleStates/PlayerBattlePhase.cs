using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattlePhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private CanvasController _canvasController;

    public bool canChangeState {  get; set; }

    public PlayerBattlePhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        canChangeState = false;
    }

    public void EnterState()
    {
        PlayerMonstersController pmc = _canvasController.playerMonstersPanel.GetComponent<PlayerMonstersController>();
        pmc.ShowCombatBtns();
    }

    public void UpdateState()
    {
        // LOGIKA ZA UPDATE STATE
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeState()
    {

    }
}
