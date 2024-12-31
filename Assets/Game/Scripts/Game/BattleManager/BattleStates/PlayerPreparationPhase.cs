using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreparationPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private EnemiesController _enemiesController;
    private PlayerController _playerController;
    private BattleManager _battleManager;
    private CanvasController _canvasController;
    private DiceManager _diceManager;

    private bool firstRoll = true;
    private bool hasNoRolls = false;

    public bool canChangeState { get; set; }
    
    public PlayerPreparationPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _enemiesController = GameManager.Instance.GetComponent<EnemiesController>();
        _playerController = PlayerController.Instance;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        _diceManager = DiceManager.Instance;
        canChangeState = false;
    }

    public void EnterState()
    {
        if(_enemiesController != null )
        {
            GameManager.Instance.GetComponent<BattleManager>().StartCoroutine(HideAbilities());
        }

        if(_battleManager != null )
        {
            _battleManager.SetCurrentAmountOfRolls();
        }

        if(_canvasController != null )
        {
            PlayerMonstersController pmc = _canvasController.playerMonstersPanel.GetComponent<PlayerMonstersController>();
            pmc.ShowMonstersDiceViewImage();
            TopUIAnimator tuia = _canvasController.UIButtonsPanel.GetComponent<TopUIAnimator>();
            tuia.ShowRolls();
        }
    }

    public void UpdateState()
    {
        if(_battleManager.CurrentAmountOfRolls == 0 || _diceManager.AllDicesAreLocked())
        {
            canChangeState = true;
        } else
        {
            if(firstRoll)
            {
                TopUIAnimator tuia = _canvasController.UIButtonsPanel.GetComponent<TopUIAnimator>();
                tuia.ShowRollBtn();
                firstRoll = false;
            }
        }
    }

    public void ExitState()
    {
        Debug.Log("Exiting player prep phase!");
        // SAKRITI ROLLS UI
        TopUIAnimator tuia = _canvasController.UIButtonsPanel.GetComponent<TopUIAnimator>();
        tuia.ShowRolls();
    }

    private IEnumerator HideAbilities()
    {
        yield return new WaitForSeconds(1f);
        _enemiesController.HideEnemiesAbilitieDetails();
    }

    public void ChangeState() 
    {
        _battleStateMachine.ChangeState<PlayerBattlePhase>();
    }

    public void RollDices()
    {
        DiceManager dm = GameManager.Instance.GetComponent<DiceManager>();
        dm.RollDices();
    }
}
