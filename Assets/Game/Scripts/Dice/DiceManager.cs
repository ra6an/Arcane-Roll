using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollState
{
    private AllyCrystalController crystal;
    private DiceRollMachineController dice;
    private bool isRolled;
    private bool reroll;

    public AllyCrystalController Crystal => crystal;
    public DiceRollMachineController Dice => dice;
    public bool IsRolled => isRolled;
    public bool Reroll => reroll;

    public DiceRollState(DiceRollMachineController _dice)
    {
        if (_dice == null) return;
        
        dice = _dice;
    }

    public void SetCrystal (AllyCrystalController _crystal)
    {
        if (_crystal == null) return;
        
        crystal = _crystal;
        isRolled = false;
        reroll = false;
    }
}

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; }

    private List<DiceRollState> diceMachineStates = new();
    [SerializeField] private List<DiceRollMachineController> diceMachines = new(4);

    public List<Transform> dicePositions = new List<Transform>();
    public List<DiceRollState> DiceMachineStates => diceMachineStates;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetStartDiceMachineStates();
    }

    public int GetRolledNumber ()
    {
        return 1;
    }

    public void SetRolledNumber(int num)
    {
        //rolledNumber = num;
    }

    private void SetStartDiceMachineStates()
    {
        foreach(DiceRollMachineController drmc in diceMachines)
        {
            if(drmc != null)
            {
                DiceRollState newDRS = new(drmc);
                diceMachineStates.Add(newDRS);
            }
        }
    }

    public void SetCrystalInStates(AllyCrystalController _cc)
    {
        if (_cc == null) return;
        bool isAdded = false;

        foreach(DiceRollState drs in diceMachineStates)
        {
            if(!isAdded && (drs.Crystal == null || !string.IsNullOrEmpty(drs.Crystal.CardData.cardName)))
            {
                drs.SetCrystal(_cc);
                isAdded = true;
            }
        }
    }

    public DiceRollState GetPositionForCard(CardSO _card)
    {
        DiceRollState drsForReturn = null;

        foreach(DiceRollState _drs in  diceMachineStates)
        {
            if(_drs.Crystal != null && _drs.Crystal.CardData == _card)
            {
                drsForReturn = _drs;
            }
        }

        return drsForReturn;
    }

    public void RollDices()
    {
        foreach(DiceRollState _drs in diceMachineStates)
        {
            DiceRollMachineController _drsMachineController = _drs.Dice;

            if(!_drs.IsRolled)
            {
                _drsMachineController.RollDice();
            }
        }
    }

    public void RerollDices()
    {
        foreach (DiceRollState _drs in diceMachineStates)
        {
            DiceRollMachineController _drsMachineController = _drs.Dice;

            if (_drs.Reroll)
            {
                _drsMachineController.RollDice();
            }
        }
    }
}
