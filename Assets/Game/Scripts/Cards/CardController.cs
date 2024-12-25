using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public CardSO cardDetails;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject spriteGO;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private GameObject[] rarityEmblems = new GameObject[4];
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;

    public void SetCard(CardSO card)
    {
        if (!card) return;

        cardDetails = card;
        title.text = cardDetails.cardName;
        health.text = cardDetails.health.ToString();
        spriteGO.GetComponent<Image>().sprite = cardDetails.art;
        SetRarity();
    }

    private void SetRarity()
    {
        foreach(GameObject emblem in rarityEmblems)
        {
            emblem.SetActive(false);
        }

        if (cardDetails.rarity == CardRarity.COMMON) rarityEmblems[0].SetActive(true);
        if (cardDetails.rarity == CardRarity.RARE) rarityEmblems[1].SetActive(true);
        if (cardDetails.rarity == CardRarity.EPIC) rarityEmblems[2].SetActive(true);
        if (cardDetails.rarity == CardRarity.LEGENDARY) rarityEmblems[3].SetActive(true);
    }

    public void FlipCard()
    {
        cardFront.SetActive(!cardFront.activeInHierarchy);
        cardBack.SetActive(!cardBack.activeInHierarchy);
    }

    public void TurnBack()
    {
        cardBack.SetActive(true);
        cardFront.SetActive(false);
    }

    public void TurnFront()
    {
        cardBack.SetActive(false);
        cardFront.SetActive(true);
    }
}
