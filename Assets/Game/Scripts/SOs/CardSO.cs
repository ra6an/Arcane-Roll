using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardRarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY
}

[CreateAssetMenu(menuName = "Data/Card")]
public class CardSO : ScriptableObject
{
    public int id;
    public string cardName;
    public Sprite art;
    public CardRarity rarity;
    public int health;
    [SerializeField] private AbilitySO[] abilities = new AbilitySO[6];

    public AbilitySO[] Abilities => abilities;
}
