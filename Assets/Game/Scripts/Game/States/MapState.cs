using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    public RoomType Type;
    public int LayerIndex;
    public int NodeIndex;
    public EnemyTeamSO EnemyTeam;
    public bool PickedPath;
    public List<Node> Connections = new List<Node>();
}

public class MapState : IGameState
{
    private StateMachine _stateMachine;
    private List<List<Node>> _map;
    private CanvasController _canvasController;

    public string StateName => "MapState";
    public Node pickedRoom { get; private set; } = null;

    public MapState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        _canvasController.HideStarterSetPanel();
    }

    public void UpdateState()
    {
        if(pickedRoom?.Type != null)
        {
            Debug.Log($"Room picked {pickedRoom.Type}");
            if (pickedRoom.Type == RoomType.BATTLE)
            {
                _stateMachine.ChangeState<BattleState>();
            } else if (pickedRoom.Type == RoomType.TREASURE)
            {
                 _stateMachine.ChangeState<TreasureRoomState>();
            } else if (pickedRoom.Type == RoomType.STORE)
            {
                _stateMachine.ChangeState<ShopState>();
            } else if (pickedRoom.Type == RoomType.BOSS)
            {
                // DODATI STATE ZA BOSS FIGHT
            } else
            {
                Debug.LogWarning("Room Type is not properly set!");
            }
        }
    }

    public void ExitState()
    {
        _canvasController.HideRoomsMapPanel();

        if (pickedRoom.Type == RoomType.BATTLE || pickedRoom.Type == RoomType.BOSS)
        {
            BattleState bs = _stateMachine.GetState<BattleState>();
            BattleRoomSO br = GameManager.Instance.GetBattleRoom();
            bs.SetEnemyTeam(pickedRoom.EnemyTeam, br);
        }
    }

    public void SetPickedRoom(Node room)
    {
        pickedRoom = room;
    }
    
    // MAP STATE LOGIC
    public void GenerateMap(int layers, int minNodes, int maxNodes)
    {
        MapSettings ms = GameManager.Instance.MapSettings;
        List<List<Node>> map = new List<List<Node>>();

        // Generiši slojeve sa nodovima
        for (int i = 0; i < layers; i++)
        {
            int nodeCount = Random.Range(minNodes, maxNodes + 1);
            List<Node> layer = new List<Node>();
            for (int j = 0; j < nodeCount; j++)
            {
                RoomType rt = GetRandomRoomType(ms.EnemyChance, ms.StoreChance, ms.TreasureChance);
                Node newNode = new Node
                {
                    Type = rt,
                    LayerIndex = i,
                    EnemyTeam = GetRandomTeam(rt),
                    NodeIndex = j,
                    PickedPath = false,
                };
                layer.Add(newNode);
            }
            map.Add(layer);
        }

        // ADDING FINAL BOSS BATTLE
        List<Node> finalLayer = new List<Node>();
        Node finalNode = new Node
        {
            Type = RoomType.BOSS,
            LayerIndex = layers,
            NodeIndex = 0,
            PickedPath = false,
        };

        finalLayer.Add(finalNode);
        map.Add(finalLayer);

        // Povezivanje nodova izme?u slojeva
        for (int i = 0; i < map.Count - 1; i++)
        {
            List<Node> currentLayer = map[i];
            List<Node> nextLayer = map[i + 1];

            int currentLayerCount = currentLayer.Count;
            int nextLayerCount = nextLayer.Count;

            // Povezivanje nodova izme?u slojeva ukoliko currentLayer ima manje nodeova od nextLayera
            if (currentLayerCount < nextLayerCount)
            {
                if (currentLayerCount == 1) // AKO TRENUTNI LAYER IMA 1 NODE
                {
                    for (int k = 0; k < nextLayerCount; k++)
                    {
                        AddConnection(currentLayer[0], nextLayer[k]);
                    }
                } // AKO LAYER IMA 1 NODE

                if (currentLayerCount == 2) // AKO LAYER IMA 2 NODEA
                {
                    // AKO JE 2:3
                    if (nextLayerCount == 3)
                    {
                        // Random odabrati kakva je veza izmedju njih
                        int helper = Random.Range(1, 4);

                        // Ako je rezultat 1 = I multy veza
                        if (helper == 1)
                        {
                            for (int k = 0; k < nextLayerCount - 1; k++)
                            {
                                AddConnection(currentLayer[0], nextLayer[k]);
                            }

                            AddConnection(currentLayer[1], nextLayer[nextLayerCount - 1]);

                        }

                        // Ako je rezultat 2 = II multy veza
                        if (helper == 2)
                        {
                            for (int k = 1; k < nextLayerCount; k++)
                            {
                                AddConnection(currentLayer[1], nextLayer[k]);
                            }

                            AddConnection(currentLayer[0], nextLayer[0]);
                        }

                        // Ako je rezultat 3 = oba nodea imaju po 2 veze
                        if (helper == 3)
                        {
                            for (int k = 0; k < nextLayerCount - 1; k++)
                            {
                                AddConnection(currentLayer[0], nextLayer[k]);
                            }
                            for (int k = 1; k < nextLayerCount; k++)
                            {
                                AddConnection(currentLayer[1], nextLayer[k]);
                            }
                        }
                    }

                    // AKO JE 2:4
                    if (nextLayerCount == 4) // AKO JE 2:4
                    {
                        // Random odabrati kakva je veza izmedju njih
                        int helper = Random.Range(1, 4);

                        // Ako je prvi multy veza (naci da li je zadnji 1 ili 2 veze)
                        if (helper == 1)
                        {
                            // Dodati multyveze na prvi node
                            for (int k = 0; k < nextLayerCount - 1; k++)
                            {
                                AddConnection(currentLayer[0], nextLayer[k]);
                            }

                            // Dodati konekcije na zadnji i opcionalno predzadnji
                            AddConnection(currentLayer[1], nextLayer[nextLayerCount - 1]);
                            if (Random.value > 0.5f)
                            {
                                AddConnection(currentLayer[1], nextLayer[nextLayerCount - 2]);
                            }
                        }

                        // Ako je drufi multy veza (naci da li je prvi 1 ili 2 veze)
                        if (helper == 2)
                        {
                            // Dodati multyveze na drugi node
                            for (int k = 1; k < nextLayerCount; k++)
                            {
                                AddConnection(currentLayer[1], nextLayer[k]);
                            }

                            // Dodati konekcije na prvi i opcionalno drugi
                            AddConnection(currentLayer[0], nextLayer[0]);
                            if (Random.value > 0.5f)
                            {
                                AddConnection(currentLayer[0], nextLayer[1]);
                            }
                        }

                        // Ako Oba imaju po 2 veze
                        if (helper == 3)
                        {
                            // Dodati dva prva node-a iz nextLayer-a kao konekcije na prvi node u current layeru
                            for (int k = 0; k < 2; k++)
                            {
                                AddConnection(currentLayer[0], nextLayer[k]);
                            }

                            // Dodati dva zadnja node-a u nextLayeru kao konekcije na drugi node u current layeru
                            for (int k = 2; k < nextLayerCount; k++)
                            {
                                AddConnection(currentLayer[1], nextLayer[k]);
                            }
                        }
                    }
                } // AKO LAYER IMA 2 NODEA

                if (currentLayerCount == 3) // AKO LAYER IMA 3 NODEA
                {
                    // Odabrati koja je vrsta konekcija
                    int helper = Random.Range(1, 5);

                    // Ako je prva multikonekcija
                    if (helper == 1)
                    {
                        // Dodati 3 konekcije na prvi node
                        for (int k = 0; k < 3; k++)
                        {
                            AddConnection(currentLayer[0], nextLayer[k]);
                        }

                        // Provjeriti da li je drugi i treci node dupla ili single konekcija
                        bool isFirstNodeSingleConnection = Random.value < 0.5f;
                        bool isSecondNodeSingleConnection = true;
                        if (isFirstNodeSingleConnection)
                        {
                            isSecondNodeSingleConnection = Random.value < 0.5f;
                        }

                        AddConnection(currentLayer[1], nextLayer[nextLayerCount - 2]);
                        AddConnection(currentLayer[2], nextLayer[nextLayerCount - 1]);

                        if (!isFirstNodeSingleConnection)
                        {
                            AddConnection(currentLayer[1], nextLayer[nextLayerCount - 1]);
                        }
                        if (!isSecondNodeSingleConnection)
                        {
                            AddConnection(currentLayer[2], nextLayer[nextLayerCount - 2]);
                        }
                    }

                    // Ako je druga multikonekcija
                    if (helper == 2)
                    {
                        // Provjeriti da li je multikonekcija na prva ili zadnja 3 noda.
                        bool isOnFirstThreeNodes = Random.value < 0.5f;

                        AddConnection(currentLayer[0], nextLayer[0]);
                        for (int k = 1; k < nextLayerCount - 1; k++)
                        {
                            AddConnection(currentLayer[1], nextLayer[k]);
                        }
                        AddConnection(currentLayer[2], nextLayer[nextLayerCount - 1]);

                        if (isOnFirstThreeNodes)
                        {
                            AddConnection(currentLayer[1], nextLayer[0]);

                            // Ukoliko je zadnji node dupla ili single konekcija
                            bool isLastMultyConnection = Random.value < 0.5f;
                            if (isLastMultyConnection)
                            {
                                AddConnection(currentLayer[currentLayerCount - 1], nextLayer[nextLayerCount - 2]);
                            }
                        }
                        else
                        {
                            AddConnection(currentLayer[1], nextLayer[nextLayerCount - 1]);

                            // Ukoliko je prvi node dupla ili single konekcija
                            bool isFirstMultyConnection = Random.value < 0.5f;
                            if (isFirstMultyConnection)
                            {
                                AddConnection(currentLayer[0], nextLayer[1]);
                            }
                        }
                    }

                    // Ako je treca multikonekcija
                    if (helper == 3)
                    {
                        // Dodati 3 konekcije na zadnji node
                        for (int k = 1; k < nextLayerCount; k++)
                        {
                            AddConnection(currentLayer[2], nextLayer[k]);
                        }

                        // Provjeriti da li je prvi i drugi node dupla ili single konekcija
                        bool isFirstNodeSingleConnection = Random.value < 0.5f;
                        bool isSecondNodeSingleConnection = true;
                        if (isFirstNodeSingleConnection)
                        {
                            isSecondNodeSingleConnection = Random.value < 0.5f;
                        }

                        AddConnection(currentLayer[0], nextLayer[0]);
                        AddConnection(currentLayer[1], nextLayer[1]);

                        if (!isFirstNodeSingleConnection)
                        {
                            AddConnection(currentLayer[0], nextLayer[1]);
                        }
                        if (!isSecondNodeSingleConnection)
                        {
                            AddConnection(currentLayer[1], nextLayer[0]);
                        }
                    }

                    // Ako svi nodeovi currentLayera imaju max 2 konekcije
                    if (helper == 4)
                    {
                        AddConnection(currentLayer[0], nextLayer[0]);
                        AddConnection(currentLayer[currentLayerCount - 1], nextLayer[nextLayerCount - 1]);

                        // Ako je srednja je single konekcija
                        bool middleNodeIsSingle = Random.value < 0.5f;

                        if (middleNodeIsSingle)
                        {
                            int connectionWithNode = Random.Range(1, 3);
                            AddConnection(currentLayer[1], nextLayer[connectionWithNode]);

                            if (connectionWithNode == 1)
                            {
                                for (int k = 2; k < nextLayerCount; k++)
                                {
                                    AddConnection(currentLayer[2], nextLayer[k]);
                                }

                                // Provjeriti da li je prvi node single ili multy
                                bool firstIsSingle = Random.value < 0.5f;
                                if (!firstIsSingle)
                                {
                                    AddConnection(currentLayer[0], nextLayer[1]);
                                }
                            }
                            else if (connectionWithNode == 2)
                            {
                                for (int k = 0; k < 2; k++)
                                {
                                    AddConnection(currentLayer[0], nextLayer[k]);
                                }

                                // Provjeriti da li je zadnji node single ili multy
                                bool lastIsSingle = Random.value < 0.5f;
                                if (!lastIsSingle)
                                {
                                    AddConnection(currentLayer[2], nextLayer[nextLayerCount - 2]);
                                }
                            }
                        }
                        else
                        {
                            for (int k = 1; k < nextLayerCount - 1; k++)
                            {
                                AddConnection(currentLayer[1], nextLayer[k]);
                            }

                            // Provjeriti vanjske nodove da li su multy
                            bool firstNodeSingle = Random.value < 0.5f;
                            bool lastNodeSingle = Random.value < 0.5f;

                            if (!firstNodeSingle)
                            {
                                AddConnection(currentLayer[0], nextLayer[1]);
                            }
                            if (!lastNodeSingle)
                            {
                                AddConnection(currentLayer[currentLayerCount - 1], nextLayer[nextLayerCount - 2]);
                            }
                        }
                    }
                } // AKO LAYER IMA 3 NODEA
            }

            // Povezivanje nodova izmedju slojeva ukoliko current i next layer imaju isto nodeova
            if (currentLayerCount == nextLayerCount)
            {
                for (int j = 0; j < currentLayerCount; j++)
                {
                    AddConnection(currentLayer[j], nextLayer[j]);
                }

                if (currentLayerCount > 2)
                {
                    // Provjeriti ima li dupla konekcija na nekom od nodeova
                    int nodeHaveMulty = Random.Range(0, currentLayerCount + 1);

                    if (nodeHaveMulty != currentLayerCount)
                    {
                        int addValue = Random.value < 0.5f ? -1 : 1;

                        if (nodeHaveMulty == 0) addValue = 1;
                        if (nodeHaveMulty == currentLayerCount - 1) addValue = -1;

                        AddConnection(currentLayer[nodeHaveMulty], nextLayer[nodeHaveMulty + addValue]);
                    }
                }
            }

            // Povezivanje nodova izmedju slojeva ukoliko currentLayer ima vise nodeova od nextLayera
            if (currentLayerCount > nextLayerCount)
            {
                // AKO NEXT LAYER IMA 1 NODE
                if (nextLayerCount == 1)
                {
                    for (int j = 0; j < currentLayerCount; j++)
                    {
                        AddConnection(currentLayer[j], nextLayer[0]);
                    }
                }

                // AKO NEXT LAYER IMA 2 NODEA
                if (nextLayerCount == 2)
                {
                    AddConnection(currentLayer[0], nextLayer[0]);
                    AddConnection(currentLayer[currentLayerCount - 1], nextLayer[1]);

                    // Ako current layer ima samo 3 nodea
                    if (currentLayerCount == 3)
                    {
                        int connections = Random.Range(0, 2);
                        AddConnection(currentLayer[1], nextLayer[connections]);
                    }

                    // Ako current layer ima 4 nodea
                    if (currentLayerCount == 4)
                    {
                        AddConnection(currentLayer[1], nextLayer[0]);
                        AddConnection(currentLayer[2], nextLayer[1]);

                        int variant = Random.Range(1, 3);

                        if (variant == 1)
                        {
                            AddConnection(currentLayer[2], nextLayer[0]);
                        }

                        if (variant == 2)
                        {
                            AddConnection(currentLayer[1], nextLayer[1]);
                        }
                    }
                }

                // AKO NEXT LAYER IMA 3 NODEA
                if (nextLayerCount == 3)
                {
                    AddConnection(currentLayer[0], nextLayer[0]);
                    AddConnection(currentLayer[currentLayerCount - 1], nextLayer[nextLayerCount - 1]);

                    bool secondNodeIsMultiple = Random.value < 0.5f;
                    bool thirdNodeIsMultiple = Random.value < 0.5f;

                    if (secondNodeIsMultiple)
                    {
                        AddConnection(currentLayer[1], nextLayer[0]);
                        AddConnection(currentLayer[1], nextLayer[1]);
                    }
                    else
                    {
                        int nodePosition = Random.Range(0, 2);
                        AddConnection(currentLayer[1], nextLayer[nodePosition]);
                    }

                    if (thirdNodeIsMultiple)
                    {
                        AddConnection(currentLayer[2], nextLayer[1]);
                        AddConnection(currentLayer[2], nextLayer[2]);
                    }
                    else
                    {
                        int nodePosition = Random.Range(1, 3);
                        AddConnection(currentLayer[2], nextLayer[nodePosition]);
                    }

                    if (!secondNodeIsMultiple && !thirdNodeIsMultiple)
                    {
                        AddConnection(currentLayer[1], nextLayer[1]);
                        AddConnection(currentLayer[2], nextLayer[1]);
                    }
                }
            }
        }

        _map = map;
        GameManager.Instance.SetMap(map);
    }

    private void AddConnection(Node _currentNode, Node _nextNode)
    {
        _currentNode.Connections.Add(_nextNode);
    }

    private RoomType GetRandomRoomType(int enemyChance, int storeChance, int treasureChance)
    {
        int totalChance = enemyChance + storeChance + treasureChance;
        int randomValue = Random.Range(0, totalChance);

        if (randomValue < enemyChance)
        {
            return RoomType.BATTLE;
        }
        else if (randomValue < enemyChance + storeChance)
        {
            return RoomType.STORE;
        }
        else
        {
            return RoomType.TREASURE;
        }
    }

    public void UpdateMap(Node _oldNode, Node _newNode)
    {
        List<List<Node>> newMap = new List<List<Node>>();
        int testNumOfPicked = 0;
        
        foreach (List<Node> _layer in _map)
        {
            List<Node> newLayer = new List<Node>();
            foreach (Node n in _layer)
            {
                Node newNode = n;
                if (_oldNode == n)
                {
                    newNode = _newNode;
                }
                if (newNode.PickedPath) { testNumOfPicked++; }
                newLayer.Add(newNode);
            }

            newMap.Add(newLayer);
        }
        _map = newMap;
        GameManager.Instance.SetMap(newMap);

        _canvasController.roomsMapPanel.GetComponent<RoomsMapController>().GenerateUI(newMap);
    }

    private EnemyTeamSO GetRandomTeam(RoomType _roomType)
    {
        if (_roomType != RoomType.BATTLE) return null;

        EnemyTeamsDB enemyTeamsDB = GameManager.Instance.EnemyTeamsDB;
        int randomTeam = Random.Range(0, enemyTeamsDB.EnemyTeams.Count);
        return enemyTeamsDB.EnemyTeams[randomTeam];
    }
}
