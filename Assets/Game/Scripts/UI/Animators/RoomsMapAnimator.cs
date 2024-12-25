using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomsMapAnimator : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 0.2f;

    private CanvasController _canvasController;
    private CanvasGroup roomsMapCanvasGroup;

    private void Awake()
    {
        GameObject _canvas = GameObject.Find("Canvas");
        _canvasController = _canvas.GetComponent<CanvasController>();

        roomsMapCanvasGroup = _canvasController.roomsMapRect.GetComponent<CanvasGroup>();

        if(roomsMapCanvasGroup == null)
        {
            roomsMapCanvasGroup = _canvasController.roomsMapRect.gameObject.AddComponent<CanvasGroup>();
        }

        roomsMapCanvasGroup.alpha = 0f;
    }

    public void ShowPanel()
    {
        RectTransform roomsMapRect = _canvasController.roomsMapPanel.GetComponent<RectTransform>();

        if (roomsMapRect == null) return;
        roomsMapRect.DOKill();

        roomsMapCanvasGroup.DOFade(1f, animationSpeed);
    }

    public void HidePanel(GameObject mapPanel)
    {
        RectTransform roomsMapRect = _canvasController.roomsMapPanel.GetComponent<RectTransform>();

        if (roomsMapRect == null) return;

        roomsMapRect.DOKill();

        roomsMapCanvasGroup.DOFade(0f, animationSpeed)
        .OnComplete(() =>
        {
            mapPanel.SetActive(false);
        });
    }
}
