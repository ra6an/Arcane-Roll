using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicsPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> relicsGO = new List<GameObject>();

    public void UpdateRelicSlots()
    {
        List<RelicSO> relics = PlayerController.Instance.CurrentGameState.relics;

        if (relics.Count == 0) return;

        for (int i = 0; i < relicsGO.Count; i++)
        {
            GameObject currentRelicGO = relicsGO[i];
            RelicSO currentRelic = null;

            if(relics.Count > i)
            {
                currentRelic = relics[i];
            }
            
            currentRelicGO.GetComponent<RelicSlot>().SetRelicSlot(currentRelic);
            
        }
    }
}
