using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Battle Room")]
public class BattleRoomSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Vector3 playerPositionFrom;
    [SerializeField] private Vector3 playerPositionTo;
    [SerializeField] private Quaternion playerRotationFrom;
    [SerializeField] private Quaternion playerRotationTo;
    [SerializeField] private Vector3 enemyPositions;
    [SerializeField] private Quaternion enemyRotation;

    public int Id => id;
    public Vector3 PlayerPositionFrom => playerPositionFrom;
    public Vector3 PlayerPositionTo => playerPositionTo;
    public Quaternion PlayerRotationFrom => playerRotationFrom;
    public Quaternion PlayerRotationTo => playerRotationTo;
    public Vector3 EnemyPositions => enemyPositions;
    public Quaternion EnemyRotation => enemyRotation;
}
