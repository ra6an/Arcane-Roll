using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlanningPhase : IBattleState
{
    private BattleStateMachine _battleStateMachine;
    private BattleManager _battleManager;
    private EnemiesController _enemiesController;
    public bool canChangeState { get; set; }

    public EnemyPlanningPhase(BattleStateMachine _bsm)
    {
        _battleStateMachine = _bsm;
        _battleManager = GameManager.Instance.GetComponent<BattleManager>();
        _enemiesController = GameManager.Instance.GetComponent<EnemiesController>();
    }

    public void EnterState()
    {
        canChangeState = false;
        //GameManager.Instance.GetComponent<BattleManager>().StartCoroutine(SetEnemiesAttack());
        _enemiesController.SetEnemiesAttacks();
    }

    public void UpdateState()
    {
        if (_enemiesController == null) return;
        if(_enemiesController.CheckIfEnemiesAreReady())
        {
            canChangeState = true;
        }
    }

    public void ExitState()
    {
        _enemiesController.HideEnemiesAbilitieDetails();
    }

    private IEnumerator SetEnemiesAttack()
    {
        yield return new WaitForSeconds(1f);
        _enemiesController.SetEnemiesAttacks();
    }

    public void ChangeState()
    {
        _battleStateMachine.ChangeState<PlayerPreparationPhase>();
    }
}
