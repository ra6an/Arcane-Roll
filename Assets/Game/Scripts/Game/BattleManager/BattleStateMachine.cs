using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BattleStateMachine 
{
    private bool isChangingState = false;
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

        isChangingState = true;

        CurrentBattleState?.UpdateState();
    }

    public void ChangeState<T>() where T : IBattleState
    {
        //CurrentBattleState?.ExitState();

        //OnBattleStateChanged.Invoke(typeof(T));

        //CurrentBattleState = _battleStates[typeof(T)];
        GameManager.Instance.StartCoroutine(ShowUIAndChangeState(typeof(T)));
    }

    private IEnumerator ShowUIAndChangeState(Type nextStateType)
    {
        if(_canvasController == null)
        {
            _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        }

        RectTransform scmr = _canvasController.stateChangeMsgRect;
        scmr.localScale = Vector3.zero;
        scmr.GetComponent<TextMeshProUGUI>().text = GetStateToString(nextStateType);
        scmr.gameObject.SetActive(true);

        scmr.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(2f);
            
        scmr.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            CurrentBattleState?.ExitState();

            OnBattleStateChanged.Invoke(nextStateType);
            
            CurrentBattleState = _battleStates[nextStateType];

            CurrentBattleState.EnterState();

            scmr.gameObject.SetActive(false);

            isChangingState = false;
        });
    }

    private string GetStateToString(Type nextStateType)
    {
        string stateToString = "";

        if(nextStateType != null && nextStateType == typeof(EnemyPlanningPhase))
        {
            stateToString = "Enemy Prep Phase";
        }

        if(nextStateType != null && nextStateType == typeof(PlayerPreparationPhase))
        {
            stateToString = "Player Prep Phase";
        }

        if (nextStateType != null && nextStateType == typeof(PlayerBattlePhase))
        {
            stateToString = "Player Combat Phase";
        }

        if (nextStateType != null && nextStateType == typeof(EnemyBattlePhase))
        {
            stateToString = "Enemy Combat Phase";
        }

        if (nextStateType != null && nextStateType == typeof(EndTurnPhase))
        {
            stateToString = "End Turn";
        }

        return stateToString;
    }
}
