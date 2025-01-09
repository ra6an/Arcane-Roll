using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Node _node;

    [SerializeField] private GameObject battleRoom;
    [SerializeField] private GameObject storeRoom;
    [SerializeField] private GameObject treasureRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject pickedPath;

    private RoomType room;

    //private bool isClickable;

    private void Update()
    {
        //CheckIfIsClickable();
    }

    private bool CheckIfIsClickable()
    {
        // MOZDA DODATI KASNIJE UKOLIKO BUDE BILO POTREBE
        if (StateMachine.Instance.CurrentState.StateName != "MapState") return false;

        MapState ms = StateMachine.Instance.GetState<MapState>();
        if (ms.pickedRoom != null) return false;

        return true;
    } // KASNIJE DODATI LOGIKU AKO BUDE BILO POTREBE

    public void SetRoom(Node node)
    {
        _node = node;
        room = node.Type;

        HideIcons();

        pickedPath.SetActive(_node.PickedPath);

        if(room == RoomType.BATTLE)
        {
            battleRoom.SetActive(true);
        } else if (room == RoomType.STORE)
        {
            storeRoom.SetActive(true);
        } else if (room == RoomType.TREASURE)
        {
            treasureRoom.SetActive(true);
        } else if (room == RoomType.BOSS)
        {
            bossRoom.SetActive(true);
            Vector2 oldPosition = transform.position;
            transform.position = new Vector2(oldPosition.x, oldPosition.y + 20);
        }
    }

    private void HideIcons()
    {
        battleRoom?.SetActive(false);
        storeRoom?.SetActive(false);
        treasureRoom?.SetActive(false);
        bossRoom?.SetActive(false);
        pickedPath?.SetActive(false);
    }

    public void SetPickedPath(bool picked)
    {
        pickedPath?.SetActive(picked);
    }

    public void HandlePickedPath()
    {
        if (!CheckIfIsClickable()) return;

        bool isAvailable = PlayerController.Instance.CheckIfNodeIsAvailableToPick(_node);

        if (!isAvailable) return;

        SetPickedPath(true);

        Node modifiedNode = new Node
        {
            Type = _node.Type,
            LayerIndex = _node.LayerIndex,
            NodeIndex = _node.NodeIndex,
            PickedPath = true,
            Rewards = _node.Rewards,
            Connections = _node.Connections,
        };

        MapState ms = StateMachine.Instance.GetState<MapState>();
        ms.SetPickedRoom(_node);
        ms.UpdateMap(_node, modifiedNode);
        PlayerController.Instance.AddPlayerPathNode(modifiedNode);
    }
}
