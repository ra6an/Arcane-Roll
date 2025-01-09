using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Node currentBattleData;
    private CanvasController _canvasController;
    private EnemyTeamSO enemyTeam;
    private int initialDiceRolls = 1;
    private int additionalDiceRolls = 0;
    private int currentAmountOfRolls = 0;
    private int currentTurn;
    private bool battleIsOver = false;
    private bool exitFromBattle = false;

    public EnemyTeamSO EnemyTeam => enemyTeam;
    public int CurrentTurn => currentTurn;
    public bool BattleIsOver => battleIsOver;
    public bool ExitFromBattle => exitFromBattle;
    public int CurrentAmountOfRolls => currentAmountOfRolls;

    private BattleStateMachine _battleStateMachine;

    private void Start()
    {
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();

        if(BattleStateMachine.Instance == null)
        {
            _battleStateMachine = new BattleStateMachine();
        } else
        {
            _battleStateMachine = BattleStateMachine.Instance;
        }

        _battleStateMachine.Initialize();
        // Subscribe na event u bsm
        _battleStateMachine.OnBattleStateChanged += HandleBattleStateChanged;
    }

    private void Update()
    {
        if (currentBattleData == null) return;
        if (!battleIsOver && _battleStateMachine != null)
        {
            CheckIfBattleIsOver();
            _battleStateMachine.Update();
        }
    }

    public void SetBattleData(Node _n)
    {
        if (_n == null) return;
        currentBattleData = _n;
    }

    private void HandleBattleStateChanged(Type newState)
    {
        if(newState == typeof(PlayerPreparationPhase))
        {
            RelicsManager.Instance.OnPlayerPreparationPhaseStarted();
        }
    }

    public void SetEnemyTeam(EnemyTeamSO et)
    {
        if(et != null)
        {
            enemyTeam = et;
        }
    }

    private void ResetBattleState()
    {
        currentTurn = 0;
        additionalDiceRolls = 0;
        currentAmountOfRolls = 0;
        battleIsOver = false;
        exitFromBattle = false;
    }

    internal void SetupBattleState(EnemyTeamSO et)
    {
        ResetBattleState();
        enemyTeam = et;
        _battleStateMachine.ChangeState<EnemyPlanningPhase>();
    }

    public IBattleState GetState()
    {
        IBattleState currState = _battleStateMachine.CurrentBattleState;

        if (currState == null) return null;

        return currState;
    }

    public void ResetAditionalDiceRolls()
    {
        additionalDiceRolls = 0;
    }

    public void AddMoreDiceRolls(int _extraRolls)
    {
        additionalDiceRolls += _extraRolls;
    }

    public void SetCurrentAmountOfRolls()
    {
        currentAmountOfRolls = initialDiceRolls + additionalDiceRolls;
    }

    public void RemoveOneRoll()
    {
        currentAmountOfRolls--;
    }

    public void SetBattleIsOver()
    {
        battleIsOver = true;
    }

    public void SetExitFromBattle(bool _value)
    {
        exitFromBattle = _value;
    }

    public void SetNextTurn()
    {
        currentTurn++;
    }

    private bool CheckIfAllyTeamIsDead()
    {
        PlayerTeamController ptc = GameManager.Instance.GetComponent<PlayerTeamController>();
        bool teamIsDead = false;
        if(ptc != null)
        {
            teamIsDead = ptc.GetAllyMonstersDamageable().Count == 0;
        }
        return teamIsDead;
    }

    private bool CheckIfEnemyTeamIsDead()
    {
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        bool teamIsDead = false;
        if (ec != null)
        {
            teamIsDead = ec.GetAllEnemies().Count == 0;
        }

        return teamIsDead;
    }

    private void CheckIfBattleIsOver()
    {
        bool enemyTeamDied = CheckIfEnemyTeamIsDead();
        bool allyTeamDied = CheckIfAllyTeamIsDead();

        if(enemyTeamDied)
        {
            battleIsOver = true;
            EndBattleController ebc = _canvasController.endBattlePanel.GetComponent<EndBattleController>();
            Debug.Log(currentBattleData);
            Debug.Log(currentBattleData.Rewards);
            ebc.SetRewards(currentBattleData.Rewards);
            _canvasController.ShowEndBattlePanel(true);
        } else if (allyTeamDied) 
        {
            battleIsOver = true;
            //EndBattleController ebc = _canvasController.endBattlePanel.GetComponent<EndBattleController>();
            //ebc.SetRewards(currentBattleData.Rewards);
            _canvasController.ShowEndBattlePanel(false);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe od evenata na bsm-u
        _battleStateMachine.OnBattleStateChanged -= HandleBattleStateChanged;
    }
}
