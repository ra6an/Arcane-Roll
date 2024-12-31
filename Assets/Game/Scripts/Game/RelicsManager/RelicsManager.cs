using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicsManager : MonoBehaviour
{
    public static RelicsManager Instance { get; private set; }

    private List<RelicSO> playersRelics = new();

    public List<RelicSO> PlayersRelics => playersRelics;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRelic(RelicSO _relic)
    {
        playersRelics.Add(_relic);
        PlayerController.Instance.AddNewRelic(_relic);
    }

    public void RemoveRelic(RelicSO _relic)
    {
        playersRelics.Remove(_relic);
        PlayerController.Instance.RemoveRelic(_relic);
    }

    public void OnPlayerPreparationPhaseStarted()
    {
        foreach (var relic in playersRelics)
        {
            if (relic.type.HasFlag(RelicEffectType.ExtraRolls))
            {
                ActivateExtraRolls(relic);
            }
        }
    }

    private void ActivateExtraRolls(RelicSO relic)
    {
        BattleManager bm = GameManager.Instance.GetComponent<BattleManager>();

        bm.AddMoreDiceRolls(relic.numOfExtraRolls);
    }
}
