using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreparationPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private EnemiesController _enemiesController;
    private PlayerController _playerController;
    private BattleManager _battleManager;
    
    public PlayerPreparationPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _enemiesController = GameManager.Instance.GetComponent<EnemiesController>();
        _playerController = PlayerController.Instance;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
    }

    public void EnterState()
    {
        if(_enemiesController != null )
        {
            GameManager.Instance.GetComponent<BattleManager>().StartCoroutine(HideAbilities());
        }

        if(_battleManager != null )
        {
            _battleManager.SetCurrentAmountOfRolls();
        }
    }

    public void UpdateState()
    {
        //throw new System.NotImplementedException();
        //_battleManager.SetCurrentAmountOfRolls();
    }

    public void ExitState()
    {
        Debug.Log("Exiting player prep phase!");
    }

    private IEnumerator HideAbilities()
    {
        yield return new WaitForSeconds(1f);
        _enemiesController.HideEnemiesAbilitieDetails();
    }
}
