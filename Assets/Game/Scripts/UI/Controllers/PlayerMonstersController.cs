using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonstersController : MonoBehaviour
{
    [SerializeField] private GameObject monstersContainerGO;
    [SerializeField] private GameObject monsterDetailsPrefab;

    public void SetupPlayersMonstersUI(DeckSO _deck)
    {
        int num = 0;
        foreach(CardSO _card in _deck.Cards)
        {
            GameObject go = Instantiate(monsterDetailsPrefab, monstersContainerGO.transform);
            go.GetComponent<MonsterDetailsController>().SetMonsterDetails(_card, num);
            num++;
        }
    }
}
