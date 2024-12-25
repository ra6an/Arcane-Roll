using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Decks")]
public class DecksDB : ScriptableObject
{
    [SerializeField] private List<DeckSO> decks = new();

    public List<DeckSO> Decks => decks;
}
