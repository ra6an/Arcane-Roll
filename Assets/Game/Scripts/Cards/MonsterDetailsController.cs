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
    [SerializeField] private TextMeshProUGUI monsterNameText;

    [Header("Health & Shield")]
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject currentHealthSliderGO;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int currentShield;
    [SerializeField] private GameObject currentShieldGO;
    [SerializeField] private TextMeshProUGUI shieldText;
    private float fillAmount;
    private Coroutine healthCoroutine;

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

    private int moveUp = 230;
    private int moveDown = 90;

    [Header("Combat Buttons")]
    [SerializeField] private GameObject combatBtnsGO;
    [SerializeField] private GameObject activateSpellBtnGO;
    [SerializeField] private Image abilitySprite;
    [SerializeField] private GameObject selectCheckBox;
    [SerializeField] private Image selectedIcon;

    [Header("Effects")]
    [SerializeField] private GameObject effectsContainerGO;
    [SerializeField] private GameObject singleEffectPrefab;

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
        UpdateEffects();

        UpdateHealth();
        UpdateShield();
    }

    private bool IsAlive()
    {
        if(diceRollState == null || diceRollState.Crystal == null) return false;
        return diceRollState.Crystal.GetComponent<Damageable>().IsAlive();
    }

    public void SetDiceRollState(DiceRollState _drs)
    {
        if (_drs == null) return;

        diceRollState = _drs;
    }

    private void HinghlightSkillBasedOnRolledNumber()
    {
        if (_currRolledNum == 0 || !IsAlive())
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
        monsterNameText.text = cd.cardName;
        //currentHealth = cardDetails.health;

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
            Transform diceTransform = null;
            foreach(Transform dt in _drs.Dice.transform)
            {
                if(dt.tag == "Dice")
                {
                    diceTransform = dt;
                }
            }
            WorldImage wi = diceWorldView.GetComponent<WorldImage>();
            wi.AddWorldObject(diceTransform);
            wi.CameraFollowTransform = true;
            
            //diceWorldView.GetComponent<WorldImage>().AddWorldObject(_drs.Dice.transform);
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

            if(healthCoroutine != null)
            {
                StopCoroutine(healthCoroutine);
            }
            
            healthCoroutine = StartCoroutine(UpdateHealthBar(fillAmount, newHealth, maxHealth));
        }
    }

    private IEnumerator UpdateHealthBar(float startHealth, int targetHealth, int maxHealth)
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            fillAmount = Mathf.Lerp(startHealth, targetHealth, elapsedTime / duration);

            currentHealthSliderGO.GetComponent<Image>().fillAmount = fillAmount / maxHealth;

            healthText.text = $"{Mathf.RoundToInt(fillAmount)} / {maxHealth}";

            yield return null;
        }

        currentHealth = targetHealth;
        //Debug.Log(currentHealth);
        currentHealthSliderGO.GetComponent<Image>().fillAmount = currentHealth / (float)maxHealth;
        healthText.text = $"{targetHealth} / {maxHealth}";
    }

    public void SetShield(int _newShield)
    {
        if(diceRollState != null)
        {
            int newShield = _newShield;

            shieldText.text = $"{newShield}";
            currentShield = newShield;
        }
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
        if (!IsAlive()) 
        {
            HideDiceWorldImage(); 
            return;
        }

        diceWorldView.SetActive(true);
        float wiHeight = worldImageContainer.GetComponent<RectTransform>().rect.height;
        int pos = moveUp;
        //worldImageContainer.transform.DOMoveY(wiHeight - 20, 0.6f);
        worldImageContainer.transform.DOMoveY(pos, 0.6f);
    }

    public void HideDiceWorldImage()
    {
        float wiHeight = worldImageContainer.GetComponent<RectTransform>().rect.height;

        int pos = moveDown;
        worldImageContainer.transform.DOMoveY(pos, 0.6f).OnComplete(() =>
        {
            diceWorldView.SetActive(false);
        });
        //worldImageContainer.transform.DOMoveY(-wiHeight + 20, 0.6f);
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
            //rolledDiceGO.SetActive(true);
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
        bool abilityIsValid = diceRollState.CheckIfAbilityIsValid();

        if(abilityIsValid)
        {
            combatBtnsGO.SetActive(true);
        } else
        {
            combatBtnsGO.SetActive(false);
        }
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
        
        Damageable dmg = diceRollState.Crystal.GetComponent<Damageable>();
        bool isAlive = dmg.IsAlive();

        if (isAlive && _playerMonstersController.ActivatedAbility.monster != null && _playerMonstersController.ActivatedAbility.monster.NeedAllyTargets > 0)
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
        if (!_abilityActivated || currentHealth <= 0) return;
        
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

    private void UpdateShield()
    {
        Damageable dmg = diceRollState.Crystal.GetComponent<Damageable>();

        if(dmg != null)
        {
            int crystalShield = dmg.GetShield();
            if(currentShield != crystalShield)
            {
                SetShield(crystalShield);
            }
        }

        if(currentShield <= 0)
        {
            currentShieldGO.SetActive(false);
        } else
        {
            currentShieldGO.SetActive(true);
        }
    }

    private void UpdateEffects()
    {
        Effectable _ef = diceRollState.Crystal.GetComponent<Effectable>();
        List<EffectBase> _effects = _ef.Effects;

        if (_effects.Count == 0 && effectsContainerGO.transform.childCount == 0) return;

        Effectable _target = diceRollState.Crystal.GetComponent<Effectable>();
        RemoveExpiredEffects(_target);

        //ClearEffectsContainer();
        foreach (EffectBase effectBase in _effects)
        {
            if (effectBase == null) continue;

            if(!CheckIfExistEffect(effectBase))
            {
                GameObject go = Instantiate(singleEffectPrefab, effectsContainerGO.transform);
                EffectController ec = go.GetComponent<EffectController>();
                ec.SetEffect(effectBase);
            }
        }
    }

    private bool CheckIfExistEffect(EffectBase _ef)
    {
        Transform exist = null;

        if (effectsContainerGO.transform.childCount == 0) return false;

        foreach (Transform child in effectsContainerGO.transform)
        {
            EffectController _ec = child.GetComponent<EffectController>();

            // AKO POSTOJI EFEKAT UPDATEATI DURATION
            if (_ec.Effect.Name == _ef.Name)
            {
                exist = child;
                _ec.SetDuration(_ef);
                if(_ef.Duration == 0) Destroy(child.gameObject);
                break;
            }
        }

        return exist != null;
    }

    private void RemoveExpiredEffects(Effectable target)
    {
        foreach(Transform child in effectsContainerGO.transform)
        {
            EffectController _ec = child.GetComponent<EffectController>();

            if (!target.EffectExistInList(_ec.Effect.Name))
            {
                Debug.Log($"Ne postoji {_ec.Effect.Name} Effekat u listi!!!");
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private void ClearEffectsContainer()
    {
        foreach(Transform child in effectsContainerGO.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
