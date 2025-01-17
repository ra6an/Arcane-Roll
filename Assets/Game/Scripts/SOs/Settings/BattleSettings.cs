using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Battle Settings")]
public class BattleSettings : ScriptableObject
{
    [SerializeField] private float lethalRange = 0.4f;
    [SerializeField] private float lifesteal = 0.3f;

    [SerializeField] private float strengthBuff = 0.25f;

    public float LethalRange => lethalRange;
    public float Lifesteal => lifesteal;
    public float StrengthBuff => strengthBuff;
}
