using DG.Tweening;
using INab.Dissolve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCrystalController : MonoBehaviour
{
    private CardSO cardData;
    private int position;
    private int rolledDice;

    public CardSO CardData => cardData;
    public int Position => position;
    public int RolledDice => rolledDice;

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
        if (_card == null || string.IsNullOrEmpty(_card.cardName)) return;
        transform.name = _card.cardName;
        cardData = _card;
        position = _position;
        transform.GetComponent<Damageable>().SetCurrentHealth(cardData.health);
    }

    public void Materialize()
    {
        transform.GetComponent<Dissolver>().Materialize();
    }

    public void Dissolve()
    {
        Debug.Log(transform.name);
        transform.GetComponent<Dissolver>().Dissolve();
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

    public void SetRolledDice(int _rolledDice)
    {
        rolledDice = _rolledDice;
    }
}
