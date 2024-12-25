using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideDetector : MonoBehaviour
{
    public string sideTouchingGround = "";
    public bool isTouchingTable = false;
    private int number;

    private void Awake()
    {
        if (this.name == "1") number = 1;
        if (this.name == "2") number = 2;
        if (this.name == "3") number = 3;
        if (this.name == "4") number = 4;
        if (this.name == "5") number = 5;
        if (this.name == "6") number = 6;
    }

    public int GetNumber()
    {
        return number;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table")) // Proverava da li je dodirnuta podloga
        {
            isTouchingTable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table")) // Proverava da li je dodirnuta podloga
        {
            isTouchingTable = false;
        }
    }
}
