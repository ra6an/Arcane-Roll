using DG.Tweening;
using Kamgam.UGUIWorldImage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetailsController : MonoBehaviour
{
    private PlayerMonstersController _playerMonstersController;
    public int id;
    public CardSO cardDetails;
    private DiceRollState diceRollState;
    [SerializeField] private GameObject monsterIconGO;
    [SerializeField] private GameObject skillsContainerGO;

    [Header("Health")]
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject currentHealthSliderGO;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Prefabs")]
    [SerializeField] private GameObject skillPrefab;

    [Header("Dice WorldView")]
    [SerializeField] private GameObject worldImageContainer;
    [SerializeField] private GameObject diceWorldView;
    [SerializeField] private GameObject rolledDiceGO;
    [SerializeField] private GameObject lockDiceBtnGO;
    [SerializeField] private GameObject iconLockedGO;
    [SerializeField] private Image rolledDiceSprite;
    [SerializeField] private List<Sprite> diceFaces = new List<Sprite>(6);

    [Header("Combat Buttons")]
    [SerializeField] private GameObject combatBtnsGO;
    [SerializeField] private GameObject activateSpellBtnGO;
    [SerializeField] private Image abilitySprite;
    [SerializeField] private GameObject selectCheckBox;
    [SerializeField] private Image selectedIcon;

    private int _currRolledNum = 0;

    private bool _abilityActivated = false;
    private int _needAllyTargets = 0;
    private int _needEnemyTargets = 0;

    public bool AbilityActivated => _abilityActivated;
    public int NeedAllyTargets => _needAllyTargets;
    public int NeedEnemyTargets => _needEnemyTargets;

    private void Awake()
    {
        CanvasController _c = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        if( _c != null )
        {
            _playerMonstersController = _c.playerMonstersPanel.GetComponent<PlayerMonstersController>();
        }
    }

    private void Update()
    {
        CheckForRolledNumbersAndSetIt();
        HinghlightSkillBasedOnRolledNumber();
        CheckIfDiceNumberIsLocked();
        ActivateAbilityWhenItIsReady();
        HandleShowCheckBox();
    }

    public void SetDiceRollState(DiceRollState _drs)
    {
        if (_drs == null) return;

        diceRollState = _drs;
    }

    private void HinghlightSkillBasedOnRolledNumber()
    {
        if (_currRolledNum == 0)
        {
            HideAllShadersFromSkills();
            HideCombatButtons();
            return;
        }

        if(_currRolledNum > 0 && _currRolledNum < 7)
        {
            HideAllShadersFromSkills();

            for (int i = 0; i < skillsContainerGO.transform.childCount; i++)
            {
                if(i == _currRolledNum - 1)
                {
                    Transform currSkill = skillsContainerGO.transform.GetChild(i);
                    currSkill.GetComponent<SkillController>().SetSkillActive();
                    SetActiveAbilityToBattleBtn(i);
                }
            }
        }
    }

    private void SetActiveAbilityToBattleBtn(int _activeAbility)
    {
        Sprite activeAbilitySprite = cardDetails.Abilities[_activeAbility].icon;
        abilitySprite.sprite = activeAbilitySprite;
    }
    //private void HideActiveAbilityBtn()
    //{

    //}

    private void HideAllShadersFromSkills()
    {
        foreach(Transform _s in skillsContainerGO.transform)
        {
            SkillController _sController = _s.GetComponent<SkillController>();

            _sController.SetSkillInactive();
        }
    }

    public void SetMonsterDetails (CardSO cd, int num)
    {
        if (cd == null) return;

        transform.name = cd.name;
        id = num;
        cardDetails = cd;
        monsterIconGO.GetComponent<Image>().sprite = cardDetails.art;
        currentHealth = cardDetails.health;

        SetHealth(currentHealth);

        foreach (AbilitySO skill in cardDetails.Abilities)
        {
            if (skill == null) continue;

            GameObject go = Instantiate(skillPrefab, skillsContainerGO.transform);
            go.GetComponent<SkillController>().SetSkill(skill);
        }

        // NACI WORLD IMAGE POSITION I DODATI GA NA UI SKRIPTU
        DiceRollState _drs = DiceManager.Instance.GetPositionForCard(cd);
        //diceWorldView.GetComponent<WorldImage>().AddWorldObject(DiceManager.Instance.dicePositions[num]);
        if(_drs != null )
        {
            diceWorldView.GetComponent<WorldImage>().AddWorldObject(_drs.Dice.transform);
        }
    }

    public void SetHealth(int newHealth)
    {
        currentHealthSliderGO.GetComponent<Image>().fillAmount = (float)newHealth / cardDetails.health;
        healthText.text = $"{newHealth} / {cardDetails.health}";
        currentHealth = newHealth;
    }

    public void SetRolledDice(int _rolledNumber)
    {
        rolledDiceGO.GetComponent<Image>().sprite = diceFaces[_rolledNumber - 1];

        SkillController sk = skillsContainerGO.transform.GetChild(_rolledNumber - 1).GetComponent<SkillController>();

        if(sk.Skill != null && !string.IsNullOrEmpty(sk.Skill.abilityName))
        {
            sk.SetSkillActive();
        }
    }

    public void ShowDiceWorldImage()
    {
        float wiHeight = worldImageContainer.GetComponent<RectTransform>().rect.height;
        
        worldImageContainer.transform.DOMoveY(wiHeight - 20, 0.6f);
        //worldImageContainer.transform.DOMoveY(wiHeight / 2, 0.6f);
    }

    public void HideDiceWorldImage()
    {
        float wiHeight = worldImageContainer.GetComponent<RectTransform>().rect.height;

        worldImageContainer.transform.DOMoveY(-wiHeight + 20, 0.6f);
        //worldImageContainer.transform.DOMoveY(-(wiHeight / 2), 0.6f);
    }

    private void CheckForRolledNumbersAndSetIt()
    {
        DiceManager dm = DiceManager.Instance;
        DiceRollState drs = dm.GetPositionForCard(cardDetails);

        if(drs.CurrRolledNumber > 0)
        {
            if(drs.CurrRolledNumber != _currRolledNum)
            {
                SetDiceSprite(drs.CurrRolledNumber);
            }
        } else
        {
            _currRolledNum = drs.CurrRolledNumber;
            rolledDiceGO.SetActive(false);
        }
    }

    public void ShowLockDiceBtn()
    {
        if (iconLockedGO.activeInHierarchy) return;
        lockDiceBtnGO.SetActive(true);
    }
    public void HideLockDiceBtn()
    {
        lockDiceBtnGO.SetActive(false);
    }

    private void SetDiceSprite(int value)
    {
        if(value > 0 && value < 7)
        {
            rolledDiceSprite.sprite = diceFaces[value - 1];
            rolledDiceGO.SetActive(true);
            _currRolledNum = value;
        }
    }

    public void LockDiceNumber()
    {
        DiceManager.Instance.LockDiceForDiceRollState(cardDetails);
    }

    private void CheckIfDiceNumberIsLocked()
    {
        if (diceRollState == null)
        {
            DiceManager dm = DiceManager.Instance;
            diceRollState = dm.GetPositionForCard(cardDetails);
            //DiceRollState drs = dm.GetPositionForCard(cardDetails);
        }

        if(diceRollState.Locked)
        {
            iconLockedGO.SetActive(true);
            if(lockDiceBtnGO.activeInHierarchy)
            {
                lockDiceBtnGO.SetActive(false);
            }
        } else
        {
            iconLockedGO.SetActive(false);
        }
    }

    public void ShowCombatButtons()
    {
        combatBtnsGO.SetActive(true);
    }
    public void HideCombatButtons()
    {
        combatBtnsGO.SetActive(false);
    }

    private void HandleShowCheckBox()
    {
        if(_playerMonstersController == null)
        {
            CanvasController _c = GameManager.Instance.Canvas.GetComponent<CanvasController>();
            if (_c != null)
            {
                _playerMonstersController = _c.playerMonstersPanel.GetComponent<PlayerMonstersController>();
            }
        }

        if (_playerMonstersController.ActivatedAbility.monster != null && _playerMonstersController.ActivatedAbility.monster._needAllyTargets > 0)
        {
            selectCheckBox.SetActive(true);
            List<Damageable> selectedTargets = _playerMonstersController.ActivatedAbility.allyTargets;
            Damageable d = diceRollState.Crystal.gameObject.GetComponent<Damageable>();
            if(selectedTargets.Contains(d))
            {
                selectedIcon.gameObject.SetActive(true);
            } else
            {
                selectedIcon.gameObject.SetActive(false);
            }
        } else
        {
            selectCheckBox.SetActive(false);
        }
    }

    public void ActivateAbility()
    {
        AbilitySO abilityToActivate = cardDetails.Abilities[_currRolledNum - 1];
        _needAllyTargets = abilityToActivate.AbilityNeedAllyTargets();
        _needEnemyTargets = abilityToActivate.AbilityNeedEnemyTargets();

        _playerMonstersController.SetActivatedAbility(this, abilityToActivate);
        _abilityActivated = true;
    }

    private void ExecuteAbility()
    {
        AbilitySO abilityToActivate = cardDetails.Abilities[_currRolledNum - 1];
        GameObject caster = diceRollState.Crystal.gameObject;
        List<Damageable> enemyTargets = diceRollState.EnemyTargets;
        List<Damageable> allyTargets = diceRollState.AllyTargets;

        abilityToActivate.Activate(caster, enemyTargets, allyTargets);
        _playerMonstersController.ActivatedAbility.ResetData();
        _abilityActivated = false;
        _currRolledNum = 0;
        diceRollState.Reset();
    }

    private void ActivateAbilityWhenItIsReady()
    {
        if (!_abilityActivated) return;
        
        if(_needAllyTargets == 0 && _needEnemyTargets == 0)
        {
            ExecuteAbility();
        }
    }

    public void OnSelectTargetClick()
    {
        if(_playerMonstersController.ActivatedAbility.monster.GetRemainingAllyTargetSpots() > 0)
        {
            Damageable d = diceRollState.Crystal.GetComponent<Damageable>();
            _playerMonstersController.ActivatedAbility.monster.SetDiceRollStateAllyTarget(d);
            _playerMonstersController.AddAllyTarget(d);
        }
    }

    public void SetDiceRollStateAllyTarget(Damageable d)
    {
        diceRollState.SetAllyTarget(d);
    }

    public int GetRemainingAllyTargetSpots()
    {
        return _needAllyTargets;
    }
    public void RemoveAllySpot()
    {
        if( _needAllyTargets > 0 ) _needAllyTargets--;
    }
    public void AddAllySpot()
    {
        AbilitySO abilityToActivate = cardDetails.Abilities[_currRolledNum - 1];
        if ( _needAllyTargets < abilityToActivate.AbilityNeedAllyTargets())
        {
            _needAllyTargets++;
        }
    }
    public int GetRemainingEnemyTargetSpots()
    {
        return _needEnemyTargets;
    }
}
