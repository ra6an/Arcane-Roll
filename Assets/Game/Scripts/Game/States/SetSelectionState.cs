using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSelectionState : IGameState
{
    private StateMachine _stateMachine;

    public string StateName => "SetSelectionState";
    private StarterSetSO pickedSet = null;

    public StarterSetSO PickedSet => pickedSet;

    public SetSelectionState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void EnterState()
    {
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();

        if(cc != null)
        {
            cc.ShowStarterSetPanel();
            PickAndShowStarterSets();
            pickedSet = ScriptableObject.CreateInstance<StarterSetSO>();
        }
    }

    public void UpdateState()
    {
        if (PickedSet == null) return;
        
        if (!string.IsNullOrEmpty(PickedSet.starterSetName))
        {
            _stateMachine.ChangeState<MapState>();
        }
    }

    public void ExitState()
    {
        PlayerController.Instance.SetStarterSet(PickedSet);
        pickedSet = null;
        CanvasController cc = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        if(cc != null)
        {
            _stateMachine.GenerateMap();
            cc.ShowRoomsMapPanel();
        }
    }

    private IEnumerator ShowSetsAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.GetComponent<StarterSetsController>().SetupStarterSets();

    }

    private void PickAndShowStarterSets()
    {
        SetsDB setsDB = GameManager.Instance.StarterSetsDB;

        if (setsDB == null || setsDB.Sets.Count < 3)
        {
            Debug.LogError("Not enough starter sets in the database!");
            return;
        }

        List<StarterSetSO> randomlyPickedSets = new();

        List<StarterSetSO> availableSets = new(setsDB.Sets);

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, availableSets.Count);
            randomlyPickedSets.Add(availableSets[randomIndex]);
            availableSets.RemoveAt(randomIndex);
        }

        StateMachine.Instance.SetStarterSets(randomlyPickedSets);
        _stateMachine.StartCoroutine(ShowSetsAfterDelay());
    }

    public void SetPickedSet(StarterSetSO set)
    {
        pickedSet = set;
    }
}
