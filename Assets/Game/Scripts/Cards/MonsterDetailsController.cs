using DG.Tweening;
using Kamgam.UGUIWorldImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetailsController : MonoBehaviour
{
    public int id;
    private CardSO cardDetails;
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
    [SerializeField] private List<Sprite> diceFaces = new List<Sprite>(6);

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
        worldImageContainer.transform.DOMoveY(wiHeight / 2 - 5f, 0.6f);
    }

    public void HideDiceWorldImage()
    {
        float wiHeight = worldImageContainer.GetComponent<RectTransform>().rect.height;
        worldImageContainer.transform.DOMoveY(-(wiHeight / 2 - 5f), 0.6f);
    }
}
