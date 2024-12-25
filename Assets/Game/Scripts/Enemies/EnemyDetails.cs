using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetails : MonoBehaviour
{
    private EnemySO enemyData;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject imageGO;

    public EnemySO EnemyData => enemyData;

    public void SetEnemyDetails(EnemySO _data)
    {
        if (_data == null) return;

        enemyData = _data;

        if(!string.IsNullOrEmpty(_data.enemyName))
        {
            titleText.text = _data.enemyName;
            imageGO.GetComponent<Image>().sprite = _data.sprite;
        }

    }
}
