using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class ParalaxCardController : MonoBehaviour
{
    private CardSO _cardData;
    [SerializeField] private GameObject cardIconGO;
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private TextMeshPro healthText;

    [Foldout("Card Rarity Emblems")]
    [SerializeField] private GameObject commonEmblemGO;
    [SerializeField] private GameObject rareEmblemGO;
    [SerializeField] private GameObject epicEmblemGO;
    [SerializeField] private GameObject legendaryEmblemGO;

    [Foldout("Card Type Emblems")]
    [SerializeField] private GameObject tankEmblemGO;
    [SerializeField] private GameObject supportEmblemGO;
    [SerializeField] private GameObject damagerEmblemGO;
    [SerializeField] private GameObject assassinEmblemGO;

    [Header("Card Rotation")]
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float maxVerticalRotationAngle = 15.0f;
    [SerializeField] private float maxHorizontalRotationAngle = 25.0f;

    private Vector3 initialMousePosition;
    private Vector3 currentRotation;

    private Coroutine resetRotationCoroutine;

    public void SetupCard(CardSO _cd)
    {
        if (_cd == null) return;
        
        _cardData = _cd;

        titleText.text = _cardData.cardName;
        cardIconGO.GetComponent<Image>().sprite = _cardData.art;
        healthText.text = _cardData.health.ToString();
        SetupRarity(_cardData.rarity);
        SetupCardType(_cardData.role);
    }

    private void SetupRarity(CardRarity cr)
    {
        commonEmblemGO.SetActive(false);
        rareEmblemGO.SetActive(false);
        epicEmblemGO.SetActive(false);
        legendaryEmblemGO.SetActive(false);

        if(cr == CardRarity.COMMON)
        {
            commonEmblemGO.SetActive(true);
        } else if(cr == CardRarity.RARE)
        {
            rareEmblemGO.SetActive(true);
        } else if(cr == CardRarity.EPIC)
        {
            epicEmblemGO.SetActive(true);
        } else if(cr == CardRarity.LEGENDARY)
        {
            legendaryEmblemGO.SetActive(true);
        }
    }

    private void SetupCardType(MonsterRole mr)
    {
        tankEmblemGO.SetActive(false);
        supportEmblemGO.SetActive(false);
        damagerEmblemGO.SetActive(false);
        assassinEmblemGO.SetActive(false);

        if(mr == MonsterRole.Tank)
        {
            tankEmblemGO.SetActive(true);
        } else if(mr == MonsterRole.Support)
        {
            supportEmblemGO.SetActive(true);
        } else if(mr == MonsterRole.Damager)
        {
            damagerEmblemGO.SetActive(true);
        } else if(mr == MonsterRole.Assasin)
        {
            assassinEmblemGO.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        initialMousePosition = Input.mousePosition;
        currentRotation = transform.localEulerAngles;
    }

    void OnMouseDrag()
    {
        Vector3 mouseDelta = Input.mousePosition - initialMousePosition;

        float xRotation = Mathf.Clamp(mouseDelta.y * rotationSpeed, -maxVerticalRotationAngle, maxVerticalRotationAngle);
        float yRotation = Mathf.Clamp(-mouseDelta.x * rotationSpeed, -maxHorizontalRotationAngle, maxHorizontalRotationAngle);

        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
    }

    void OnMouseUp()
    {
        transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.OutQuad);
    }
}
