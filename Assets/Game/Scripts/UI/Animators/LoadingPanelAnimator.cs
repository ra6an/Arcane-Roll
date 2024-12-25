using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform loadingContent;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float exitAnimationDuration = 1f;

    private CanvasController _canvasController;
    private CanvasGroup loadingContentGroup;

    private void Awake()
    {
        if (_canvasController == null)
        {
            _canvasController = FindObjectOfType<CanvasController>();
        }
    }

    private void Start()
    {
        loadingContentGroup = _canvasController.loadingContentRect.GetComponent<CanvasGroup>();

        if (loadingContentGroup == null)
        {
            loadingContentGroup = _canvasController.loadingContentRect.gameObject.AddComponent<CanvasGroup>();
        }

        loadingContentGroup.alpha = 0f;
    }

    public void ShowPanel()
    {
        if (loadingContent == null) return;

        if (loadingContentGroup == null)
        {
            Debug.LogWarning("loadingContentGroup is null. Attempting reinitialization...");
            loadingContentGroup = _canvasController.loadingContentRect?.GetComponent<CanvasGroup>();

            if (loadingContentGroup == null && _canvasController.loadingContentRect != null)
            {
                loadingContentGroup = _canvasController.loadingContentRect.gameObject.AddComponent<CanvasGroup>();
            }

            if (loadingContentGroup == null)
            {
                Debug.LogError("Failed to initialize loadingContentGroup!");
                return;
            }
        }

        CanvasGroup cg = _canvasController.loadingPanel.GetComponent<CanvasGroup>();

        if (cg == null)
        {
            cg = _canvasController.loadingPanel.AddComponent<CanvasGroup>();
        }

        cg.alpha = 1;
        _canvasController.loadingPanel.SetActive(true);

        loadingContentGroup.DOKill();
        loadingContentGroup.DOFade(1f, animationDuration);
    }

    public void HidePanel()
    {
        if(loadingContent == null) return;

        loadingContentGroup.DOKill();

        CanvasGroup cg = _canvasController.loadingPanel.GetComponent<CanvasGroup>();

        if(cg == null)
        {
            _canvasController.loadingPanel.AddComponent<CanvasGroup>();
        }

        cg.DOFade(0f, exitAnimationDuration).OnComplete(() =>
        {
            _canvasController.loadingPanel.SetActive(false);
        });
    }
}
