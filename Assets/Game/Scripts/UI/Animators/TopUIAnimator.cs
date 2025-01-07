using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform mapButtonRect;
    [SerializeField] private RectTransform coinsRect;
    [SerializeField] private RectTransform battleStateRect;
    [SerializeField] private RectTransform rollBtnRect;
    //[SerializeField] private RectTransform rollsRect;

    [SerializeField] private int distance = 160;
    [SerializeField] private float animDuration = 0.3f;
    //[SerializeField] private float delayBetweenAnim = 0.1f;

    private bool isVisible = false;
    private bool rollBtnVisible = false;
    private Vector2 initialPosition;
    private RectTransform panelRT;

    private void Awake()
    {
        panelRT = transform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (panelRT != null)
        {
            initialPosition = panelRT.anchoredPosition;
        }
    }

    public void ShowTopUI()
    {
        //if(mapButtonRect == null ||  coinsRect == null) return;

        if (isVisible) return;

        //mapButtonRect.DOKill();
        //coinsRect.DOKill();
        RectTransform rt = transform.GetComponent<RectTransform>();

        rt.DOKill();
        rt.DOAnchorPos(new Vector2(initialPosition.x - distance, initialPosition.y), animDuration).SetEase(Ease.InOutQuad);
        //relicsPanel.DOAnchorPos(new Vector2(initialPosition.x + moveDistance, initialPosition.y), animationSpeed).SetEase(Ease.InOutQuad);
        isVisible = true;
        //mapButtonRect.localScale = Vector3.zero;
        //mapButtonRect.DOScale(Vector3.one, animDuration)
        //    .SetEase(Ease.OutBack);

        //coinsRect.localScale = Vector3.zero;
        //coinsRect.DOScale(Vector3.one, animDuration)
        //    .SetEase(Ease.OutBack)
        //    .SetDelay(delayBetweenAnim);
    }

    public void HideTopUI()
    {
        //if (mapButtonRect == null || coinsRect == null) return;

        if(!isVisible) return;

        RectTransform rt = transform.GetComponent<RectTransform>();

        rt.DOKill();
        //rt.DOMoveX(initialPosition.x + distance, animDuration).SetEase(Ease.OutBack);
        rt.DOAnchorPos(new Vector2(initialPosition.x + distance, initialPosition.y), animDuration).SetEase(Ease.InOutQuad);
        isVisible = false;

        //isVisible = false;

        //mapButtonRect.DOKill();
        //coinsRect.DOKill();

        //mapButtonRect.localScale = Vector3.one;
        //mapButtonRect.DOScale(Vector3.zero, animDuration)
        //    .SetEase(Ease.OutBack);

        //coinsRect.localScale = Vector3.one;
        //coinsRect.DOScale(Vector3.zero, animDuration)
        //    .SetEase(Ease.OutBack)
        //    .SetDelay(delayBetweenAnim);
    }

    public void ShowRolls()
    {
        BattlePhaseUIController bpuic = battleStateRect.GetComponent<BattlePhaseUIController>();
        bpuic.ShowRolls();
    }

    public void HideRolls()
    {
        BattlePhaseUIController bpuic = battleStateRect.GetComponent<BattlePhaseUIController>();
        bpuic.HideRolls();
    }

    public void ShowRollBtn()
    {
        if (rollBtnRect == null) return;

        if (rollBtnVisible) return;

        rollBtnRect.DOKill();
        Debug.Log("POKAZI BUTTON ZA ROLL DICES!!!!!!!!!!!!!!!!");
        rollBtnRect.DOScale(Vector2.one, animDuration).SetEase(Ease.OutBack);
        rollBtnVisible = true;
    }

    public void HideRollBtn()
    {
        if (rollBtnRect == null) return;

        if (!rollBtnVisible) return;

        rollBtnRect.DOKill();

        rollBtnRect.DOScale(Vector2.zero, animDuration).SetEase(Ease.OutBack);
        rollBtnVisible = false;
    }

    public void RollBtn()
    {
        DiceManager dm = DiceManager.Instance;
        BattleManager bm = GameManager.Instance.GetComponent<BattleManager>();

        if (dm == null) return;

        int currAmountOfRolls = bm.CurrentAmountOfRolls;
        IBattleState currState = bm.GetState();

        if(currState is PlayerPreparationPhase && currAmountOfRolls > 0)
        {
            dm.RollDices();
        }
    }
}
