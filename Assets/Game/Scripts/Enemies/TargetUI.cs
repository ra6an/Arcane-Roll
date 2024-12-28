using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : MonoBehaviour
{
    [SerializeField] private GameObject targetSpriteGO;

    public void SetTarget(Damageable _target)
    {
        CardSO data = _target.GetComponent<AllyCrystalController>().CardData;

        targetSpriteGO.GetComponent<Image>().sprite = data.art;
    }

    public void AnimateIn()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
}
