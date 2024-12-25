using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Relics")]
public class RelicsDB : ScriptableObject
{
    [SerializeField] private List<RelicSO> relics;

    public List<RelicSO> Relics => relics;
}
