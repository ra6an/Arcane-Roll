using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattlePhaseUIController : MonoBehaviour
{
    [SerializeField] private GameObject shaderGO;
    [SerializeField] private TextMeshProUGUI battlePhaseStateText;

    [Header("Rolls UI")]
    [SerializeField] private GameObject rollsGO;
    [SerializeField] private TextMeshProUGUI rollsText;

    private BattleManager _battleManager;

    private bool rollsShown = false;

    private void Update()
    {
        if (_battleManager == null)
        {
            _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        } else
        {
            IBattleState currState = _battleManager.GetState();

            if(currState != null)
            {
                ShaderHandler();
            }

            int caor = _battleManager.CurrentAmountOfRolls;
            if(caor.ToString() != rollsText.text)
            {
                rollsText.text = caor.ToString();
            }
        }
    }

    public void SetRollsRemain(int _remainingRolls)
    {
        rollsText.text = _remainingRolls.ToString();
    }

    public void ShowRolls()
    {
        if (rollsShown) return;

        RectTransform rollsRT = rollsGO.GetComponent<RectTransform>();
        rollsRT.DOKill();

        rollsRT.DOMoveX(rollsGO.transform.position.x - (rollsRT.rect.width + 30), 0.3f).SetEase(Ease.InCubic);
        rollsShown = true;
    }

    public void HideRolls()
    {
        if (!rollsShown) return;

        RectTransform rollsRT = rollsGO.GetComponent<RectTransform>();
        rollsRT.DOKill();

        rollsRT.DOMoveX(rollsGO.transform.position.x + (rollsRT.rect.width + 30), 0.3f).SetEase(Ease.InCubic);
        rollsShown = false;
    }

    public void ShowShader()
    {
        shaderGO.SetActive(true);
    }
    public void HideShader()
    {
        shaderGO.SetActive(false);
    }

    private void ShaderHandler()
    {
        BattleManager bm = GameManager.Instance.GetComponent<BattleManager>();
        IBattleState currState = bm.GetState();

        if(currState.canChangeState)
        {
            ShowShader();
        }
        else
        {
            HideShader();
        }
    }

    public void ChangeState()
    {
        // LOGIKA ZA MJENJANJE BATTLE STATEA
        BattleManager bm = GameManager.Instance.GetComponent<BattleManager>();
        IBattleState currState = bm.GetState();

        if(currState.canChangeState)
        {
            currState.ChangeState();
        }
    }
}
