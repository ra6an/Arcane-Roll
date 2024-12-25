using System.Collections;
using System.Collections.Generic;
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
        _canvas.playerMonstersPanel.GetComponent<PlayerMonstersController>().SetupPlayersMonstersUI(_starterSet.deck);
    }
}
