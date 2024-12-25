using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleState
{
    void EnterState();
    void UpdateState();
    void ExitState();
}
