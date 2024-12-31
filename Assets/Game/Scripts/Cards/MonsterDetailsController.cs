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
    public int id;
    public CardSO cardDetails;
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

    private int _currRolledNum = 0;

    private void Update()
    {
        CheckForRolledNumbersAndSetIt();
        HinghlightSkillBasedOnRolledNumber();
        CheckIfDiceNumberIsLocked();
    }

    private void HinghlightSkillBasedOnRolledNumber()
    {
        if (_currRolledNum == 0)
        {
            HideAllShadersFromSkills();
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
                }
            }
        }
    }

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

        if(drs.CurrRolledNumber > 0 && drs.CurrRolledNumber != _currRolledNum)
        {
            SetDiceSprite(drs.CurrRolledNumber);
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
        DiceManager dm = DiceManager.Instance;
        DiceRollState drs = dm.GetPositionForCard(cardDetails);

        if(drs.Locked)
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
}
