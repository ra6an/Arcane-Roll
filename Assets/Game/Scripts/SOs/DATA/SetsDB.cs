using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DB/Sets")]
public class SetsDB : ScriptableObject
{
    [SerializeField] private List<StarterSetSO> sets;

    public List<StarterSetSO> Sets => sets;
}
