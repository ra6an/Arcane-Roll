using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemySO : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public string enemyName;
    public GameObject enemyPrefab;
    public int health;
    public int attack;
    public int coinsReward = 5;
    [SerializeField] private AbilitySO[] abilities = new AbilitySO[0];

    public AbilitySO[] Abilities => abilities;
}
