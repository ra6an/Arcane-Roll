using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Deck")]
public class DeckSO : ScriptableObject
{
    public int id;
    public string deckName;
    public Sprite art;
    [SerializeField] private CardSO[] cards = new CardSO[4];

    public CardSO[] Cards => cards;
}
