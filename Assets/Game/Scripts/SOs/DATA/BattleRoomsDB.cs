using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Battle Rooms")]
public class BattleRoomsDB : ScriptableObject
{
    [SerializeField] private List<BattleRoomSO> battleRooms;

    public List<BattleRoomSO> BattleRooms => battleRooms;
}
