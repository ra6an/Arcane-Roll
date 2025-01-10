using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopUIController : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] private TextMeshProUGUI coinsText;
    private int currnetMoney = 0;

    private void Awake()
    {
        playerController = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateCoinsStatus();
    }

    private void UpdateCoinsStatus()
    {
        if(playerController == null)
        {
            playerController = PlayerController.Instance;
        }

        int playerMoney = playerController.CurrentGameState.playersMoney;

        if (currnetMoney != playerMoney)
        {
            currnetMoney = playerMoney;
            coinsText.text = currnetMoney.ToString();
        }
    }
}
