using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    private AbilitySO skill;
    [SerializeField] private GameObject skillSpriteGO;
    [SerializeField] private GameObject skillNoneGO;
    [SerializeField] private GameObject activeShaderGO;

    public void SetSkill(AbilitySO skill)
    {
        if (skill == null) return;

        if (skill.abilityName == "")
        {
            skillNoneGO.SetActive(true);
            skillSpriteGO.SetActive(false);
        } else
        {
            skillNoneGO.SetActive(false);

            skillSpriteGO.GetComponent<Image>().sprite = skill.icon;

            skillSpriteGO.SetActive(true);
        }
    }

    public void SetSkillActive()
    {
        activeShaderGO.SetActive(true);
    }

    public void SetSkillInactive()
    {
        activeShaderGO.SetActive(false);
    }
}
