using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Relic")]
public class RelicSO : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string relicName;
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
}
