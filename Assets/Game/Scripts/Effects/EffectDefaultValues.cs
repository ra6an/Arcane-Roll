using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDefaultValues
{
    // BUFFS
    private float strength = 0.25f;
    private float vitality = 0.3f;
    private float resilient = 0.4f;

    // DEBUFFS
    private float weak = 0.25f;
    private float vulnerable = 0.4f;
    private float brittle = 0.3f;

    // STATUS
    private int burn = 4;
    private int poison = 3;
    private int chill = 2;

    public float Strength => strength;
    public float Vitality => vitality;
    public float Resilient => resilient;
    public float Weak => weak;
    public float Vulnerable => vulnerable;
    public float Brittle => brittle;
    public int Burn => burn;
    public int Poison => poison;
    public int Chill => chill;
}
