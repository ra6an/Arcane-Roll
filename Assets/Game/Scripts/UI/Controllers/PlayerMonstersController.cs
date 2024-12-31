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

    public void AddPlayerMonster(CardSO _card, int position)
    {
        GameObject go = Instantiate(monsterDetailsPrefab, monstersContainerGO.transform);
        go.GetComponent<MonsterDetailsController>().SetMonsterDetails(_card, position);
    }

    public void ShowMonstersDiceViewImage()
    {
        foreach(Transform mon in monstersContainerGO.transform)
        {
            if(mon == null) continue;
            MonsterDetailsController mdc = mon.GetComponent<MonsterDetailsController>();

            if(mdc != null)
            {
                mdc.ShowDiceWorldImage();
            }
        }
    }

    public void ShowLockDiceBtn(CardSO _card)
    {
        foreach(Transform mon in monstersContainerGO.transform)
        {
            if(mon == null) continue;
            MonsterDetailsController mdc = mon.GetComponent<MonsterDetailsController>();

            if(mdc != null && _card.id == mdc.cardDetails.id)
            {
                mdc.ShowLockDiceBtn();
            }
        }
    }

    public void HideLockBtns()
    {
        foreach (Transform mon in monstersContainerGO.transform)
        {
            if (mon == null) continue;
            MonsterDetailsController mdc = mon.GetComponent<MonsterDetailsController>();

            mdc.HideLockDiceBtn();
        }
    }
}
