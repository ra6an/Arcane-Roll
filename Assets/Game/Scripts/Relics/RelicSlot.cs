using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class RelicSlot : MonoBehaviour
{
    private RelicSO relic;
    [SerializeField] private GameObject relicImageGO;

    public void SetRelicSlot(RelicSO _relic)
    {
        relic = _relic;

        if (relic == null)
        {
            relicImageGO.GetComponent<Image>().sprite = null;
            relicImageGO.SetActive(false);
        } else
        {
            relicImageGO.GetComponent<Image>().sprite = relic.icon;
            relicImageGO.SetActive(true);
        }
    }
}
