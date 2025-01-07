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

        UpdateHealth();
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

    public void SetHealth(int _newHealth)
    {
        if(diceRollState != null)
        {
            int newHealth = _newHealth;
            Damageable dmg = diceRollState.Crystal.GetComponent<Damageable>();
            int maxHealth = dmg.MaxHealth;

            if(newHealth > maxHealth) newHealth = maxHealth;

            healthText.text = $"{newHealth} / {maxHealth}";
            currentHealth = newHealth;

            //Postaviti health bar

            currentHealthSliderGO.GetComponent<Image>().fillAmount = newHealth / (float)maxHealth;
        }
        //currentHealthSliderGO.GetComponent<Image>().fillAmount = (float)newHealth / cardDetails.health;
        //healthText.text = $"{newHealth} / {cardDetails.health}";
        //currentHealth = newHealth;
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

        if (_playerMonstersController.ActivatedAbility.monster != null && _playerMonstersController.ActivatedAbility.monster.NeedAllyTargets > 0)
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

        // OVO PROBATI KADA BROJ ZIVIH IGRACA JE MANJI OD TRAZENOG BROJA TARGETA //
        
        PlayerTeamController ptc = GameManager.Instance.GetComponent<PlayerTeamController>();
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        if(ptc != null)
        {
            List<Damageable> aliveAllyMonsters = ptc.GetAllyMonstersDamageable();
            if(_needAllyTargets >= aliveAllyMonsters.Count)
            {
                _needAllyTargets = aliveAllyMonsters.Count;
                foreach(Damageable d in aliveAllyMonsters)
                {
                    _playerMonstersController.ActivatedAbility.monster.SetDiceRollStateAllyTarget(d);
                    _playerMonstersController.AddAllyTarget(d);
                }
            }
        }
        if(ec != null)
        {
            List<Damageable> aliveEnemyMonsters = ec.GetAllEnemies();
            if(_needEnemyTargets >= aliveEnemyMonsters.Count)
            {
                _needEnemyTargets = aliveEnemyMonsters.Count;
                foreach (Damageable d in aliveEnemyMonsters)
                {
                    Debug.Log($"Damageable: {d.transform.name} !!!!!!!!!!!!!!!!!!");
                    _playerMonstersController.ActivatedAbility.monster.SetDiceRollStateEnemyTarget(d);
                    _playerMonstersController.AddEnemyTarget(d);
                }
            }
        }
    }

    private void ExecuteAbility()
    {
        AbilitySO abilityToActivate = cardDetails.Abilities[_currRolledNum - 1];
        GameObject caster = diceRollState.Crystal.gameObject;
        List<Damageable> enemyTargets = diceRollState.EnemyTargets;
        List<Damageable> allyTargets = diceRollState.AllyTargets;

        if(enemyTargets.Count == 0)
        {
            // Dodati logiku za ubacivanje svih zivih enemy damageable script-i
            EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
            List<Damageable> aliveEnemyMonsters = ec.GetAllEnemies(); // Vraca samo alive enemiese
            foreach(Damageable aem in aliveEnemyMonsters)
            {
                SetDiceRollStateEnemyTarget(aem);
            }
        }

        if(allyTargets.Count == 0)
        {
            // Dodati logiku za ubacivanje svih zivih ally damageable script-i
            PlayerTeamController ptc = GameManager.Instance.GetComponent<PlayerTeamController>();
            List<Damageable> aliveAllyMonsters = ptc.GetAllyMonstersDamageable();
            foreach(Damageable aam in aliveAllyMonsters)
            {
                SetDiceRollStateAllyTarget(aam);
            }
        }

        abilityToActivate.Activate(caster, enemyTargets, allyTargets);
        _playerMonstersController.ActivatedAbility.ResetData();
        _abilityActivated = false;
        _currRolledNum = 0;
        diceRollState.ActivateAbility();
        //diceRollState.Reset();
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
            if (_playerMonstersController.ActivatedAbility.allyTargets.Contains(d)) return;
            _playerMonstersController.ActivatedAbility.monster.SetDiceRollStateAllyTarget(d);
            _playerMonstersController.AddAllyTarget(d);
        }
    }

    public void SetDiceRollStateAllyTarget(Damageable d)
    {
        diceRollState.SetAllyTarget(d);
    }
    public void SetDiceRollStateEnemyTarget(Damageable d)
    {
        Debug.Log($"Prije dodavanja {d}");
        diceRollState.SetEnemyTarget(d);
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
    public void RemoveEnemySpot()
    {
        if( _needEnemyTargets > 0 ) _needEnemyTargets--;
    }
    public void AddEnemySpot()
    {
        AbilitySO abilityToActivate = cardDetails.Abilities[_currRolledNum - 1];
        if (_needEnemyTargets < abilityToActivate.AbilityNeedEnemyTargets())
        {
            _needEnemyTargets++;
        }
    }
    public int GetRemainingEnemyTargetSpots()
    {
        return _needEnemyTargets;
    }

    private void UpdateHealth()
    {
        if(diceRollState == null) return;

        Damageable dmg = diceRollState.Crystal.GetComponent<Damageable>();

        if( dmg != null )
        {
            int crystalHealth = dmg.GetHealth();
            if(currentHealth != crystalHealth)
            {
                SetHealth(crystalHealth);
            }
        }
    }
}
