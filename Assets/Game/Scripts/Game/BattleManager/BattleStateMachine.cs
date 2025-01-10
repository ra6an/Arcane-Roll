using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStateMachine 
{
    private CanvasController _canvasController;
    public static BattleStateMachine Instance { get; private set; }
    public IBattleState CurrentBattleState { get; private set; }

    private Dictionary<Type, IBattleState> _battleStates;

    // EVENTS
    public event Action<Type> OnBattleStateChanged = delegate { };

    public BattleStateMachine()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("Multiple instances of BattleStateMachine detected!");
        }
    }

    public void Initialize()
    {
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        Debug.Log("INICIJALIZACIJA!!!!!!!!!!!!");
        _battleStates = new Dictionary<Type, IBattleState>
        {
            { typeof(EnemyPlanningPhase), new EnemyPlanningPhase(this) },
            { typeof(PlayerPreparationPhase), new PlayerPreparationPhase(this) },
            { typeof(PlayerBattlePhase), new PlayerBattlePhase(this) },
            { typeof(EnemyBattlePhase), new EnemyBattlePhase(this) },
            { typeof(EndTurnPhase), new EndTurnPhase(this) },
        };
    }

    public void Update()
    {
        if (CurrentBattleState == null) return;
        CurrentBattleState?.UpdateState();
    }

    public void ChangeState<T>() where T : IBattleState
    {
        CurrentBattleState?.ExitState();

        OnBattleStateChanged.Invoke(typeof(T));

        CurrentBattleState = _battleStates[typeof(T)];
        GameManager.Instance.StartCoroutine(ShowUIAndChangeState());
        //CurrentBattleState.EnterState();
    }

    private IEnumerator ShowUIAndChangeState()
    {
        if(_canvasController == null)
        {
            _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        }

        RectTransform scmr = _canvasController.stateChangeMsgRect;
        scmr.localScale = Vector3.zero;
        scmr.GetComponent<TextMeshProUGUI>().text = GetStateToString();
        scmr.gameObject.SetActive(true);

        scmr.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(2f);
            
        scmr.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            CurrentBattleState.EnterState();
            scmr.gameObject.SetActive(false);
        });
        
    }

    private string GetStateToString()
    {
        string stateToString = "";

        if(CurrentBattleState != null && CurrentBattleState.GetType() == typeof(EnemyPlanningPhase))
        {
            stateToString = "Enemy Prep Phase";
        }

        if(CurrentBattleState != null && CurrentBattleState.GetType() == typeof(PlayerPreparationPhase))
        {
            stateToString = "Player Prep Phase";
        }

        if (CurrentBattleState != null && CurrentBattleState.GetType() == typeof(PlayerBattlePhase))
        {
            stateToString = "Player Combat Phase";
        }

        if (CurrentBattleState != null && CurrentBattleState.GetType() == typeof(EnemyBattlePhase))
        {
            stateToString = "Enemy Combat Phase";
        }

        if (CurrentBattleState != null && CurrentBattleState.GetType() == typeof(EndTurnPhase))
        {
            stateToString = "End Turn";
        }

        return stateToString;
    }
}
