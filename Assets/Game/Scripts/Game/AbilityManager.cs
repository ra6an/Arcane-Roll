using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("Ability Manager initialized!");
    }

    public void ActivateAbility(AbilitySO ability, GameObject monster, GameObject[] targets)
    {
        if(ability != null)
        {
            ability.Activate(monster, targets);
        } else
        {
            Debug.LogWarning("No ability assigned.");
        }
    }
}
