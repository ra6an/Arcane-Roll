using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AbilitySO skill;
    [SerializeField] private GameObject skillSpriteGO;
    [SerializeField] private GameObject skillNoneGO;
    [SerializeField] private GameObject activeShaderGO;
    [SerializeField] private GameObject abilityTypePrefab;
    [SerializeField] private GameObject abilityDescriptionGO;
    [SerializeField] private GameObject abilityTypesContainerGO;
    [SerializeField] private TextMeshProUGUI abilityTitleText;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;
    [SerializeField] private GameObject abilityFillGO;

    public AbilitySO Skill => skill;

    private void Awake()
    {
        abilityDescriptionGO.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill == null) return;
        RectTransform rt = abilityDescriptionGO.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;

        abilityDescriptionGO.SetActive(true);
        rt.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (skill == null) return;
        RectTransform rt = abilityDescriptionGO.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        rt.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            abilityDescriptionGO.SetActive(false);
        });
    }

    public void SetSkill(AbilitySO _skill)
    {
        if (_skill == null) return;

        if (string.IsNullOrEmpty(_skill.abilityName))
        {
            skillNoneGO.SetActive(true);
            skillSpriteGO.SetActive(false);
        } else
        {
            skill = _skill;
            skillNoneGO.SetActive(false);
            skillSpriteGO.GetComponent<Image>().sprite = skill.icon;
            skillSpriteGO.SetActive(true);

            abilityTitleText.text = skill.abilityName;
            abilityDescriptionText.text = skill.description;

            GenerateAbilityTypeIcons(skill.type);
        }

    }

    public void SetSkillActive()
    {
        activeShaderGO.SetActive(true);
        abilityFillGO.SetActive(true);
    }

    public void SetSkillInactive()
    {
        activeShaderGO.SetActive(false);
        abilityFillGO.SetActive(false);
    }

    private void GenerateAbilityTypeIcons(AbilityType _a)
    {
        foreach(AbilityType flag in Enum.GetValues(typeof(AbilityType)))
        {
            if(flag == AbilityType.None) continue;

            if(_a.HasFlag(flag))
            {
                GameObject go = Instantiate(abilityTypePrefab, abilityTypesContainerGO.transform);
                AbilityTypeController atc = go.GetComponent<AbilityTypeController>();
                atc.SetSingleAbilityType(flag);
            }
        }
    }
}
