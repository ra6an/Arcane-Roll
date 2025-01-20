using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    private EffectBase effect;
    [SerializeField] private GameObject effectIcon;
    [SerializeField] private TextMeshProUGUI effectDuration;
    private CanvasController _canvasController;

    public EffectBase Effect => effect;

    private void Awake()
    {
        _canvasController = GameManager.Instance.Canvas.GetComponent<CanvasController>();
    }

    public void SetEffect(EffectBase _eff)
    {
        if (_eff == null) return;

        effect = _eff;
        SetIcon(effect);
        effectDuration.text = _eff.Duration.ToString();
    }

    private void SetIcon(EffectBase _eff)
    {
        Image icon = effectIcon.GetComponent<Image>();
        if(_eff.Name == "Strength")
        {
            icon.sprite = _canvasController.Strength;
        }
        if (_eff.Name == "Weak")
        {
            icon.sprite = _canvasController.Weak;
        }
        if (_eff.Name == "Vitality")
        {
            icon.sprite = _canvasController.Vitality;
        }
        if (_eff.Name == "Vulnerable")
        {
            icon.sprite = _canvasController.Vulnerable;
        }
        if (_eff.Name == "Resilient")
        {
            icon.sprite = _canvasController.Resilient;
        }
        if (_eff.Name == "Brittle")
        {
            icon.sprite = _canvasController.Brittle;
        }

        // Status Effects
        if (_eff.Name == "Burn")
        {
            icon.sprite = _canvasController.Burn;
        }
        if (_eff.Name == "Poison")
        {
            icon.sprite = _canvasController.Poison;
        }
        if (_eff.Name == "Chill")
        {
            icon.sprite = _canvasController.Chill;
        }
    }

    internal void SetDuration(EffectBase eb)
    {
        effect = eb;
        effectDuration.text = eb.Duration.ToString();
    }
}
