using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollMachineController : MonoBehaviour
{
    private CardSO cardData;
    [SerializeField] private GameObject diceGO;

    public CardSO CardData => cardData;

    public int GetRolledNumber()
    {
        DiceStats diceStats = diceGO.GetComponent<DiceStats>();
        return diceStats.side;
    }

    public void RollDice()
    {
        diceGO.GetComponent<Dice>().RollDice();
    }
}
