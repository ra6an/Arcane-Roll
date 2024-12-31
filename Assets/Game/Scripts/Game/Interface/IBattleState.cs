using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleState
{
    bool canChangeState { get; set; }
    void EnterState();
    void UpdateState();
    void ExitState();
    void ChangeState();
}
