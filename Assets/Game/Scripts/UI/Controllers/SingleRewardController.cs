using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleRewardController : MonoBehaviour
{
    private RewardContext context;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amount;

    [SerializeField] private Sprite coinsSprite;

    public void SetSingleReward(RewardContext _context)
    {
        if (_context == null) return;

        context = _context;

        if(context.type == RewardType.Coin)
        {
            iconImage.sprite = coinsSprite;
            amount.text = context.amount.ToString();
        }

        if(context.type == RewardType.Relic && context.relic != null)
        {
            iconImage.sprite = context.relic.icon;
            amount.text = context.amount.ToString();
        }

        if(context.type == RewardType.Ability && context.ability != null)
        {
            iconImage.sprite = context.ability.icon;
            amount.text = context.amount.ToString();
        }
    }
}
