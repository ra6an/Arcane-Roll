using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemySO : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public string enemyName;
    public int health;
    public int attack;
    [SerializeField] private AbilitySO[] abilities = new AbilitySO[6];

    public AbilitySO[] Abilities => abilities;
}
