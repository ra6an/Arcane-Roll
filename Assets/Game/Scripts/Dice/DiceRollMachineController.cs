using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollMachineController : MonoBehaviour
{
    private CardSO cardData;
    private bool startedRolling = false;
    [SerializeField] private GameObject diceGO;

    public CardSO CardData => cardData;

    private void Update()
    {
        if(startedRolling)
        {
            Dice _dice = diceGO.GetComponent<Dice>();
            if(_dice != null && _dice.rolledNumber != 0)
            {
                int _rolledNumber = _dice.GetRolledNumber();
                startedRolling = false;
                DiceManager.Instance.SetRolledNumberToDiceRollState(cardData, _rolledNumber);
            }
        }
    }

    public int GetRolledNumber()
    {
        DiceStats diceStats = diceGO.GetComponent<DiceStats>();
        return diceStats.side;
    }

    public void RollDice()
    {
        diceGO.GetComponent<Dice>().RollDice();
        startedRolling = true;
    }

    internal void SetupDiceRollMachineController(CardSO _c)
    {
        if (_c == null) return;

        cardData = _c;
    }
}
