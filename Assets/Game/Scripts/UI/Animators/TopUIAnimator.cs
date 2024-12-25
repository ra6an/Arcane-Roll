using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform mapButtonRect;
    [SerializeField] private RectTransform coinsRect;

    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private float delayBetweenAnim = 0.1f;

    private bool isVisible = false;

    public void ShowTopUI()
    {
        if(mapButtonRect == null ||  coinsRect == null) return;

        if (isVisible) return;

        isVisible = true;

        mapButtonRect.DOKill();
        coinsRect.DOKill();

        mapButtonRect.localScale = Vector3.zero;
        mapButtonRect.DOScale(Vector3.one, animDuration)
            .SetEase(Ease.OutBack);

        coinsRect.localScale = Vector3.zero;
        coinsRect.DOScale(Vector3.one, animDuration)
            .SetEase(Ease.OutBack)
            .SetDelay(delayBetweenAnim);
    }

    public void HideTopUI()
    {
        if (mapButtonRect == null || coinsRect == null) return;

        if(!isVisible) return;

        isVisible = false;

        mapButtonRect.DOKill();
        coinsRect.DOKill();
        
        mapButtonRect.localScale = Vector3.one;
        mapButtonRect.DOScale(Vector3.zero, animDuration)
            .SetEase(Ease.OutBack);

        coinsRect.localScale = Vector3.one;
        coinsRect.DOScale(Vector3.zero, animDuration)
            .SetEase(Ease.OutBack)
            .SetDelay(delayBetweenAnim);
    }
}
