using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSetsController : MonoBehaviour
{
    [SerializeField] private GameObject setsContainer;
    [SerializeField] private GameObject setPrefab;

    public void SetupStarterSets()
    {
        List<StarterSetSO> sets = StateMachine.Instance.StarterSets;
        foreach (StarterSetSO set in sets)
        {
            GameObject go = Instantiate(setPrefab, setsContainer.transform);
            go.GetComponent<Set>().SetupStarterSet(set);
        }
    }

    public void RemoveStarterSets()
    {
        foreach (Transform child in setsContainer.GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
    }
}
