using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Starter Set")]
public class StarterSetSO : ScriptableObject
{
    public int id;
    public string starterSetName;
    public DeckSO deck;
    public RelicSO relic;
}
