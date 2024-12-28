using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlanningPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private BattleManager _battleManager;
    private EnemiesController _enemiesController;

    public EnemyPlanningPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _enemiesController = GameManager.Instance.GetComponent<EnemiesController>();
    }

    public void EnterState()
    {
        Debug.Log("Entering enemy prep phase!");
        //_enemiesController.SetEnemiesAttacks();
        GameManager.Instance.GetComponent<BattleManager>().StartCoroutine(SetEnemiesAttack());
    }

    public void UpdateState()
    {
        if (_enemiesController == null) return;
        if(_enemiesController.CheckIfEnemiesAreReady())
        {
            _battleStateMachine.ChangeState<PlayerPreparationPhase>();
        }
    }

    public void ExitState()
    {
        Debug.Log("Exiting enemy prep phase!");
    }

    private IEnumerator SetEnemiesAttack()
    {
        yield return new WaitForSeconds(1f);
        _enemiesController.SetEnemiesAttacks();
    }
}
