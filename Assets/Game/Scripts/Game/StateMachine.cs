using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance { get; private set; }

    public bool playerCanPickRoom = true;
    public CanvasController _canvas;

    [SerializeField] private List<StarterSetSO> starterSets;

    [SerializeField] private GameObject relicsContainerGO;

    [Header("Testing items")]
    [SerializeField] private RelicSO relic;

    private Dictionary<Type, IGameState> _states;

    public IGameState CurrentState { get; private set; }
    public List<StarterSetSO> StarterSets => starterSets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("State Machine initialized!");
            return;
        }

        Instance = this;

        _canvas = GameManager.Instance.Canvas.GetComponent<CanvasController>();
    }

    private void Start()
    {
        _states = new Dictionary<Type, IGameState>
        {
            { typeof(LoadingState), new LoadingState(this) },
            { typeof(SetSelectionState), new SetSelectionState(this) },
            { typeof(MapState), new MapState(this) },
            { typeof(BattleState), new BattleState(this) },
            { typeof(ShopState), new ShopState(this) },
            { typeof(TreasureRoomState), new TreasureRoomState(this) },
            { typeof(GameOverState), new GameOverState(this) },
        };

        //GenerateMap();
        ChangeState<LoadingState>();
    }

    public void GenerateMap()
    {
        MapState ms = GetState<MapState>();

        if (ms != null)
        {
            MapSettings _mSettings = GameManager.Instance.MapSettings;
            ms.GenerateMap(_mSettings.Layers, _mSettings.MinNodes, _mSettings.MaxNodes);
        }
    }

    public void SetStarterSet(StarterSetSO set)
    {
        SetSelectionState sss = GetState<SetSelectionState>();
        if (sss != null)
        {
            sss.SetPickedSet(set);
        }
    }

    public void ChangeState<T>() where T : IGameState
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }
        //Debug.Log($"Changing state from: {CurrentState} to: {typeof(T).Name}");

        CurrentState = _states[typeof(T)];
        CurrentState.EnterState();
        //Debug.Log($"State changed to: {CurrentState}");
    }

    private void Update()
    {
        CurrentState?.UpdateState();
    }

    public T GetState<T>() where T : class, IGameState
    {
        _states.TryGetValue(typeof(T), out var state);
        return state as T;
    }

    internal void SetStarterSets(List<StarterSetSO> _starterSets)
    {
        starterSets = _starterSets;
    }
}
