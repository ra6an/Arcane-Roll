using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Battle Settings")]
public class BattleSettings : ScriptableObject
{
    [SerializeField] private float lethalRange = 0.4f;
    [SerializeField] private float lifesteal = 0.3f;

    public float LethalRange => lethalRange;
    public float Lifesteal => lifesteal;
}
