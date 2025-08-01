using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattlePhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private BattleManager _battleManager;
    private CanvasController _canvasController;
    private DiceManager _diceManager;

    public bool canChangeState {  get; set; }

    public PlayerBattlePhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        _diceManager = GameManager.Instance.GetComponent<DiceManager>();
    }

    public void EnterState()
    {
        canChangeState = false;
        PlayerMonstersController pmc = _canvasController.playerMonstersPanel.GetComponent<PlayerMonstersController>();
        pmc.ShowCombatBtns();
    }

    public void UpdateState()
    {
        // LOGIKA ZA UPDATE STATE
        if(_diceManager.CheckIfAllAbilitiesAreActivated())
        {
            canChangeState = true;
        }
    }

    public void ExitState()
    {
        _diceManager.ResetAllDiceRollStates();
    }

    public void ChangeState()
    {
        _battleStateMachine.ChangeState<EnemyBattlePhase>();
    }
}
