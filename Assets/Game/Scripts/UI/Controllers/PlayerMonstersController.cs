using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivatedAbility
{
    public MonsterDetailsController monster;
    public AbilitySO ability;
    public List<Damageable> enemyTargets = new();
    public List<Damageable> allyTargets = new();

    public void SetData(MonsterDetailsController mdc, AbilitySO a)
    {
        monster = mdc;
        ability = a;
        enemyTargets.Clear();
        allyTargets.Clear();
    }

    public void AddEnemy(Damageable target)
    {
        enemyTargets.Add(target);
    }
    public void RemoveEnemy(Damageable target)
    {
        enemyTargets.Remove(target);
    }
    public void AddAlly(Damageable target)
    {
        allyTargets.Add(target);
    }
    public void RemoveAlly(Damageable target)
    {
        allyTargets.Remove(target);
    }

    public void ResetData()
    {
        monster = null;
        ability = null;
        enemyTargets.Clear();
        allyTargets.Clear();
    }
}

public class PlayerMonstersController : MonoBehaviour
{
    [SerializeField] private GameObject monstersContainerGO;
    
    [SerializeField] private GameObject monsterDetailsPrefab;

    private ActivatedAbility activatedAbility = new();

    public ActivatedAbility ActivatedAbility => activatedAbility;

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
    public void HideMonstersDiceViewImage()
    {
        foreach (Transform mon in monstersContainerGO.transform)
        {
            if (mon == null) continue;
            MonsterDetailsController mdc = mon.GetComponent<MonsterDetailsController>();

            if (mdc != null)
            {
                mdc.HideDiceWorldImage();
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

    public void ShowCombatBtns()
    {
        foreach (Transform mon in monstersContainerGO.transform)
        {
            if (mon == null) continue;
            MonsterDetailsController mdc = mon.GetComponent<MonsterDetailsController>();
            mdc.ShowCombatButtons();
        }
    }

    public void SetActivatedAbility(MonsterDetailsController mdc, AbilitySO _ability)
    {
        activatedAbility.SetData(mdc, _ability);
    }

    public void AddAllyTarget(Damageable _target)
    {
        activatedAbility.AddAlly(_target);
        activatedAbility.monster.RemoveAllySpot();
    }

    public void RemoveAllyTarget(Damageable _target)
    {
        activatedAbility.RemoveAlly(_target);
        activatedAbility.monster.AddAllySpot();
    }
    public void AddEnemyTarget(Damageable _target)
    {
        activatedAbility.AddEnemy(_target);
        activatedAbility.monster.RemoveEnemySpot();
    }
    public void RemoveEnemySpot(Damageable _target)
    {
        activatedAbility.RemoveEnemy(_target);
        activatedAbility.monster.AddEnemySpot();
    }

    public bool AbilityIsActivated()
    {
        return activatedAbility.ability != null;
    }
}
