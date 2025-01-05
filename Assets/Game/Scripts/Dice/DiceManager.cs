using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollState
{
    private AllyCrystalController crystal;
    private DiceRollMachineController dice;
    private List<Damageable> allyTargets = new();
    private List<Damageable> enemyTargets = new();
    private int currRolledNumber;
    private bool isRolled;
    private bool locked;

    public AllyCrystalController Crystal => crystal;
    public DiceRollMachineController Dice => dice;
    public List<Damageable> AllyTargets => allyTargets;
    public List<Damageable > EnemyTargets => enemyTargets;
    public int CurrRolledNumber => currRolledNumber;
    public bool IsRolled => isRolled;
    public bool Locked => locked;

    public DiceRollState(DiceRollMachineController _dice)
    {
        if (_dice == null) return;
        
        dice = _dice;
        locked = false;
        isRolled = false;
        allyTargets = new();
        enemyTargets = new();
    }

    public void SetCrystal (AllyCrystalController _crystal)
    {
        if (_crystal == null) return;
        
        crystal = _crystal;
        isRolled = false;
    }

    public void SetAllyTarget (Damageable target)
    {
        allyTargets.Add(target);
    }

    public void SetEnemyTarget (Damageable target)
    {
        enemyTargets.Add(target);
    }

    public void SetRolledNumber(int _num)
    {
        currRolledNumber = _num;
    }

    public void SetIsRolled(bool _isRolled)
    {
        isRolled = _isRolled;
    }

    public void SetLocked(bool _locked)
    {
        locked = _locked;
    }

    public void Reset()
    {
        locked = false;
        currRolledNumber = 0;
        isRolled = false;
        allyTargets.Clear();
        enemyTargets.Clear();
        Debug.Log(allyTargets.Count);
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
        
        foreach(DiceRollState drs in diceMachineStates)
        {
            if (drs.Crystal != null) continue;
            
            drs.SetCrystal(_cc);
            drs.Dice.SetupDiceRollMachineController(_cc.CardData);
            break;
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
        // LOGIKA ZA SAKRIVANJE LOCK BTNA
        GameObject playerMonstersUI = GameManager.Instance.Canvas.GetComponent<CanvasController>().playerMonstersPanel;
        if (playerMonstersUI != null)
        {
            playerMonstersUI.GetComponent<PlayerMonstersController>().HideLockBtns();
        }

        // LOKIGA ZA ROLL
        foreach (DiceRollState _drs in diceMachineStates)
        {
            DiceRollMachineController _drsMachineController = _drs.Dice;

            if(_drsMachineController != null && !_drs.Locked)
            {
                _drsMachineController.RollDice();
                _drs.SetIsRolled(true);
            }
        }
        GameManager.Instance.GetComponent<BattleManager>().RemoveOneRoll();
    }

    public void SetRolledNumberToDiceRollState(CardSO _c, int _rolledNumber)
    {
        foreach(DiceRollState _drs in diceMachineStates)
        {
            if(_c.id == _drs.Dice.CardData.id)
            {
                _drs.SetRolledNumber(_rolledNumber);
                GameObject playerMonstersUI = GameManager.Instance.Canvas.GetComponent<CanvasController>().playerMonstersPanel;
                if (playerMonstersUI != null)
                {
                    playerMonstersUI.GetComponent<PlayerMonstersController>().ShowLockDiceBtn(_c);
                }
            }
        }
    }

    public void ResetAllDiceRollStates()
    {
        foreach (DiceRollState _drs in DiceMachineStates)
        {
            _drs.Reset();
        }
    }

    public void LockAllDices()
    {
        foreach(DiceRollState _drs in DiceMachineStates)
        {
            if (!_drs.Locked) _drs.SetLocked(true);
        }
    }

    public void UnlockAllDices()
    {
        foreach (DiceRollState _drs in DiceMachineStates)
        {
            if (_drs.Locked) _drs.SetLocked(false);
        }
    }

    public void LockDiceForDiceRollState(CardSO _c)
    {
        foreach (DiceRollState _drs in diceMachineStates)
        {
            if (_c.id == _drs.Dice.CardData.id)
            {
                _drs.SetLocked(true);
            }
        }
    }

    public bool AllDicesAreLocked()
    {
        bool allDicesAreLocked = true;

        foreach(DiceRollState _drs in diceMachineStates)
        {
            if(!_drs.Locked)
            {
                allDicesAreLocked = false;
                break;
            }
        }

        return allDicesAreLocked;
    }
}
