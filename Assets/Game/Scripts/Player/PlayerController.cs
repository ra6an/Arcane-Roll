using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameState
{
    public DeckSO deck;
    public List<Node> playersPath = new List<Node>();
    public List<RelicSO> relics = new List<RelicSO>();
    public int playersMoney = 100;
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private CanvasController _canvas;

    [Header("Testing items")]
    [SerializeField] private RelicSO relic;

    private GameState currentGameState;

    public GameState CurrentGameState => currentGameState;

    [SerializeField] private float transformDuration = 1f;
    [SerializeField] private float rotationDuration = 0.8f;

    [SerializeField] private GameObject crystalsContainer;
    [SerializeField] private float crystalsContainerDistance = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GameObject _canvasGO = GameManager.Instance.Canvas;
        
        if (_canvasGO != null)
        {
            _canvas = _canvasGO.GetComponent<CanvasController>();
            ResetStateMachine();
        }
    }

    private void Update()
    {
        MoveCrystalsContainer();
    }

    private void AddNewRelic(RelicSO r)
    {
        if (currentGameState.relics.Count == 6) return;
        
        if (_canvas == null) return;
        currentGameState.relics.Add(r);
        _canvas.relicsContainerGO.GetComponent<RelicsPanel>().UpdateRelicSlots();
    }

    private void ResetStateMachine()
    {
        GameState newGameState = new()
        {
            deck = ScriptableObject.CreateInstance<DeckSO>(),
            playersPath = new List<Node>(),
            relics = new List<RelicSO>(),
            playersMoney = 100,
        };

        currentGameState = newGameState;
    }

    public void AddPlayerPathNode(Node node)
    {
        currentGameState.playersPath.Add(node);
    }

    public bool CheckIfNodeIsAvailableToPick(Node n)
    {
        List<List<Node>> _map = GameManager.Instance.Map;

        if (_map == null) return false;

        List<Node> _playersPath = currentGameState.playersPath;

        if (_playersPath.Count == 0 && _map[0].Contains(n)) return true;

        if (_playersPath.Count > 0 && _playersPath[^1].Connections.Contains(n))
        {
            return true;
        }

        return false;
    }

    public void SetStarterSet(StarterSetSO _starterSet)
    {
        AddNewRelic(_starterSet.relic);
        currentGameState.deck = _starterSet.deck;
        //SetupAllyMonsters();
        //_canvas.playerMonstersPanel.GetComponent<PlayerMonstersController>().SetupPlayersMonstersUI(_starterSet.deck);
    }

    public void SetupAllyMonsters()
    {
        GameManager.Instance.GetComponent<PlayerTeamController>().SetAllyTeam();
    }

    internal void SetPlayerPosition(BattleRoomSO battleRoom)
    {
        Vector3 newPosition = battleRoom.PlayerPositionTo;
        transform.localPosition = newPosition;
    }

    public void MovePlayerOnBattleStart(BattleRoomSO battleRoom, Action onComplete = null)
    {
        transform.SetLocalPositionAndRotation(battleRoom.PlayerPositionFrom, battleRoom.PlayerRotationFrom);

        transform.DOLocalMove(battleRoom.PlayerPositionTo, transformDuration)
           .SetEase(Ease.InOutQuad)
           .OnComplete(() =>
           {
               // Izvrši dodatni kod nakon završetka animacije
               onComplete?.Invoke();
           });

        // Rotiraj kameru prema ciljnoj rotaciji
        transform.DOLocalRotate(battleRoom.PlayerRotationTo.eulerAngles, rotationDuration)
            .SetEase(Ease.InOutQuad);
    }

    private void MoveCrystalsContainer()
    {
        if (crystalsContainer == null) return;

        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        crystalsContainer.transform.position = playerPosition + forwardDirection * crystalsContainerDistance;
        crystalsContainer.transform.rotation = transform.rotation;
    }

    internal void GetAvailableRolls()
    {
        int _availableRolls = 1;

        List<RelicSO> _relics = currentGameState.relics;

        
    }
}
