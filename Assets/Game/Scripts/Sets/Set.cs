using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Set : MonoBehaviour
{
    private StarterSetSO starterSetData;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI setTitle;
    [SerializeField] private TextMeshProUGUI deckTitle;
    [SerializeField] private GameObject deckSlot;
    [SerializeField] private TextMeshProUGUI relicTitle;
    [SerializeField] private GameObject relicIcon;

    [Header("Prefabs")]
    [SerializeField] private GameObject cardPrefab;

    public void SetupStarterSet(StarterSetSO _ss)
    {
        if (_ss == null) return;

        starterSetData = _ss;

        setTitle.text = starterSetData.starterSetName;
        relicTitle.text = starterSetData.relic.relicName;
        relicIcon.GetComponent<Image>().sprite = starterSetData.relic.icon;

        // Instantiate deck and set it to deck slot gameObject
        GameObject go = Instantiate(cardPrefab, deckSlot.transform);
        go.GetComponent<CardController>().TurnBack();
    }

    public void SetPickedSet()
    {
        StateMachine.Instance.SetStarterSet(starterSetData);
    }
}
