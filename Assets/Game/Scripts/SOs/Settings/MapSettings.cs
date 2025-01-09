using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Settings/Map Settings")]
public class MapSettings : ScriptableObject
{
    [Header("Room Type Chance")]
    [SerializeField] private int enemyChance = 85;
    [SerializeField] private int treasureChance = 5;
    [SerializeField] private int storeChance = 10;
    [Header("Rewards Chance")]
    [SerializeField] private int abilityChance = 5;
    [SerializeField] private int relicChance = 5;
    [Header("Map Generator Values")]
    [SerializeField] private int layers = 12;
    [Range(1, 3)]
    [SerializeField] private int minNodes = 1;
    [Range(2, 4)]
    [SerializeField] private int maxNodes = 4;
    [Header("Map UI")]
    [SerializeField] private int _xSpacing = 150;
    [SerializeField] private int _ySpacing = 120;
    [SerializeField] private int _bottomPadding = 100;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int lineWidth = 10;
    [SerializeField] private Sprite lineSprite;
    [SerializeField] private Color lineColor;
    [SerializeField] private Color battleColor;
    [SerializeField] private Color storeColor;
    [SerializeField] private Color treasureColor;
    [SerializeField] private Color bossColor;

    // Public properties for accessing private fields
    public int EnemyChance => enemyChance;
    public int TreasureChance => treasureChance;
    public int StoreChance => storeChance;

    public int AbilityChance => abilityChance;
    public int RelicChance => relicChance;

    public int Layers => layers;
    public int MinNodes => minNodes;
    public int MaxNodes => maxNodes;

    public int XSpacing => _xSpacing;
    public int YSpacing => _ySpacing;
    public int BottomPadding => _bottomPadding;

    public GameObject RoomPrefab => roomPrefab;
    public int LineWidth => lineWidth;
    public Sprite LineSprite => lineSprite;

    public Color LineColor => lineColor;
    public Color BattleColor => battleColor;
    public Color StoreColor => storeColor;
    public Color TreasureColor => treasureColor;
    public Color BossColor => bossColor;

    private void OnValidate()
    {
        if (minNodes >= maxNodes)
        {
            Debug.LogWarning("Variable minNodes cannot be greater than or equal to maxNodes!");
            minNodes = maxNodes - 1;
        }

        if (minNodes < 1)
        {
            Debug.LogWarning("Variable minNodes cannot be less than 1. Adjusting to 1...");
            minNodes = 1;
        }
    }
}
