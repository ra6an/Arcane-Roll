using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndBattleController : MonoBehaviour
{
    //private bool victoryActive = false;
    //private bool defeatActive = false;
    private float panelScale = 0.4f;

    [SerializeField] private GameObject victoryGO;
    [SerializeField] private GameObject defeatGO;
    [SerializeField] private float animDuration = 0.3f;

    [Header("Victory Elements")]
    [SerializeField] private GameObject rewardsContainerGO;
    [SerializeField] private GameObject singleRewardPrefab;

    public void SetRewards(List<RewardContext> rc)
    {
        ClearRewardsContainer();

        if (rc.Count <= 0) return;

        foreach (RewardContext context in rc)
        {
            GameObject go = Instantiate(singleRewardPrefab, rewardsContainerGO.transform);
            SingleRewardController src = go.GetComponent<SingleRewardController>();
            if(src != null)
            {
                src.SetSingleReward(context);
            }
        }
    }

    private void ClearRewardsContainer()
    {
        foreach(Transform child in rewardsContainerGO.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowEndBattlePanel(bool isVictory)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null) return;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        transform.gameObject.SetActive(true);

        canvasGroup.DOFade(1f, 1.5f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            if(isVictory)
            {
                ShowVictoryScreen();
            } else
            {
                ShowDefeatScreen();
            }
        });
    }

    public void HideEndBattlePanel()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null) return;

        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            transform.gameObject.SetActive(false);
        });
    }

    public void ShowVictoryScreen()
    {
        victoryGO.SetActive(true);
        RectTransform vRect = victoryGO.GetComponent<RectTransform>();
        vRect.DOKill();

        vRect.DOScale(panelScale, animDuration).SetEase(Ease.OutQuart);
    }
    public void  HideVictoryScreen()
    {
        RectTransform vRect = victoryGO.GetComponent<RectTransform>();
        vRect.DOKill();

        vRect.DOScale(Vector3.zero, animDuration).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            HideEndBattlePanel();
        });
    }

    public void ShowDefeatScreen()
    {
        defeatGO.SetActive(true);
        RectTransform dRect = defeatGO.GetComponent<RectTransform>();
        dRect.DOKill();

        dRect.DOScale(panelScale, animDuration).SetEase(Ease.OutQuart);
    }
    public void HideDefeatScreen()
    {
        RectTransform dRect = defeatGO.GetComponent<RectTransform>();
        dRect.DOKill();

        dRect.DOScale(Vector3.zero, animDuration).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            HideEndBattlePanel();
        });
    }

    public void EndGame()
    {
        Debug.Log("Zavrsen run!!!!");
        // OVDJE IDE LOGIKA ZA KRAJ RUNA
        HideDefeatScreen();
    }

    public void ExitBattleAndOpenMap()
    {
        BattleManager bm = GameManager.Instance.GetComponent<BattleManager>();
        if (bm == null) return;
        HideVictoryScreen();
        bm.SetExitFromBattle(true);
        HideVictoryScreen();
    }
}
