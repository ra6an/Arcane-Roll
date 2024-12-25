using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; }

    private int rolledNumber = 0;

    public List<Transform> dicePositions = new List<Transform>();
    //public IEnumerable<Transform> diceTransforms => dicePositions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetRolledNumber ()
    {
        return rolledNumber;
    }

    public void SetRolledNumber(int num)
    {
        rolledNumber = num;
    }
}
