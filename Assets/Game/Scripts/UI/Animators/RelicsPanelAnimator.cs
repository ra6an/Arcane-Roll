using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicsPanelAnimator : MonoBehaviour
{
    [SerializeField] private float moveDistance = 140f;
    [SerializeField] private float animationSpeed = 0.5f;

    private RectTransform relicsPanel;
    private Vector2 initialPosition;

    private void Awake()
    {
        relicsPanel = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if(relicsPanel != null)
        {
            initialPosition = relicsPanel.anchoredPosition;
        }
    }

    public void ShowPanel()
    {
        if (relicsPanel == null) return;

        relicsPanel.DOKill();

        relicsPanel.DOAnchorPos(new Vector2(initialPosition.x + moveDistance, initialPosition.y), animationSpeed).SetEase(Ease.InOutQuad);
    }

    public void HidePanel()
    {
        if(relicsPanel == null) return;

        relicsPanel.DOKill();

        relicsPanel.DOAnchorPos(initialPosition, animationSpeed).SetEase(Ease.InOutQuad);
    }
}
