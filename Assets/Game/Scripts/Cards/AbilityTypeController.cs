using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTypeController : MonoBehaviour
{
    public AbilityType _abilityType;
    [SerializeField] private Image icon;

    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite defenseIcon;
    [SerializeField] private Sprite healIcon;
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private Sprite debuffIcon;
    [SerializeField] private Sprite crowdControllIcon;

    public void SetSingleAbilityType(AbilityType a)
    {
        if (a == AbilityType.None)
        {
            Debug.LogWarning("Ignoring AbilityType.None");
            return;
        }

        _abilityType = a;

        if(a.HasFlag(AbilityType.Attack))
        {
            icon.sprite = attackIcon;
        }

        if(a.HasFlag(AbilityType.Defense))
        {
            icon.sprite = defenseIcon;
        }

        if(a.HasFlag(AbilityType.Heal))
        {
            icon.sprite = healIcon;
        }

        if(a.HasFlag(AbilityType.Buff))
        {
            icon.sprite = buffIcon;
        }

        if(a.HasFlag(AbilityType.Debuff))
        {
            icon.sprite = debuffIcon;
        }

        if(a.HasFlag(AbilityType.CrowdControl))
        {
            icon.sprite = crowdControllIcon;
        }
    }
}
