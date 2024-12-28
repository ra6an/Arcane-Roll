using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Foldout("DataBase")]
    [SerializeField] private SetsDB starterSetsDB;
    [SerializeField] private EnemyTeamsDB enemyTeamsDB;
    [SerializeField] private BattleRoomsDB battleRoomsDB;

    private GameObject canvas;

    private List<List<Node>> _map;

    [Foldout("Game Settings")]
    [SerializeField] private MapSettings mapSettings;

    public GameObject Canvas => canvas;
    public List<List<Node>> Map => _map;
    public MapSettings MapSettings => mapSettings;
    public SetsDB StarterSetsDB => starterSetsDB;
    public EnemyTeamsDB EnemyTeamsDB => enemyTeamsDB;
    public BattleRoomsDB BattleRoomsDB => battleRoomsDB;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GetCanvas();

        Debug.Log("Game Manager initialized!");
    }

    private void GetCanvas()
    {
        GameObject _canvas = GameObject.Find("Canvas");

        if(_canvas != null)
        {
            canvas = _canvas;
        }
    }

    public void SetMap(List<List<Node>> _m)
    {
        _map = _m;
    }

    public BattleRoomSO GetBattleRoom()
    {
        int rndInt = 0;

        if(battleRoomsDB.BattleRooms.Count > 1)
        {
            rndInt = Random.Range(0, battleRoomsDB.BattleRooms.Count);
        }

        return battleRoomsDB.BattleRooms[rndInt];
    }
}
