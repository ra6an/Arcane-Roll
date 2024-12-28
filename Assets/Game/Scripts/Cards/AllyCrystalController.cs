using DG.Tweening;
using INab.Dissolve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCrystalController : MonoBehaviour
{
    private CardSO cardData;
    private int position;

    public CardSO CardData => cardData;
    public int Position => position;

    private void Awake()
    {
        this.GetComponent<Dissolver>().currentState = Dissolver.DissolveState.Dissolved;
        
    }

    private void Start()
    {
        StartRotation();
        StartLevitation();
    }

    public void SetMonster(CardSO _card, int _position)
    {
        if (string.IsNullOrEmpty(_card.cardName)) return;
        cardData = _card;
        position = _position;
        transform.GetComponent<Damageable>().SetCurrentHealth(cardData.health);
    }

    public void Materialize()
    {
        this.GetComponent<Dissolver>().Materialize();
    }

    public void Dissolve()
    {
        this.GetComponent<Dissolver>().Dissolve();
    }

    private void StartRotation()
    {
        transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    private void StartLevitation()
    {
        transform.DOMoveY(transform.position.y + 0.2f, 3f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
