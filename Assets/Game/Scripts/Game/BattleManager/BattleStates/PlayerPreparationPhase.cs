using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreparationPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private EnemiesController _enemiesController;
    private PlayerController _playerController;

    private int numOfRolls = 1;
    
    public PlayerPreparationPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _enemiesController = GameManager.Instance.GetComponent<EnemiesController>();
        _playerController = PlayerController.Instance;
    }

    public void EnterState()
    {
        Debug.Log("Entering player prep phase!");

        if(_enemiesController != null )
        {
            GameManager.Instance.GetComponent<BattleManager>().StartCoroutine(HideAbilities());
        }

        if(_playerController != null )
        {
            _playerController.GetAvailableRolls();
        }
    }

    public void UpdateState()
    {
        //throw new System.NotImplementedException();
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
