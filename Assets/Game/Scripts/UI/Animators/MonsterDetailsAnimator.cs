using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetailsAnimator : MonoBehaviour
{
    [SerializeField] private float moveDistance = 125f;
    [SerializeField] private float animationSpeed = 0.5f;

    private RectTransform monsterDetailsRect;
    private Vector2 initialPosition;

    private void Awake()
    {
        monsterDetailsRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (monsterDetailsRect != null)
        {
            initialPosition = monsterDetailsRect.anchoredPosition;
        }
    }

    public void ShowPanel()
    {
        if (monsterDetailsRect == null) return;

        monsterDetailsRect.DOKill();

        monsterDetailsRect.DOAnchorPos(new Vector2(initialPosition.x, initialPosition.y + moveDistance), animationSpeed).SetEase(Ease.InOutQuad);
    }

    public void HidePanel()
    {
        if (monsterDetailsRect == null) return;

        monsterDetailsRect.DOKill();

        monsterDetailsRect.DOAnchorPos(new Vector2(initialPosition.x, initialPosition.y - moveDistance), animationSpeed).SetEase(Ease.InOutQuad);
    }
}
