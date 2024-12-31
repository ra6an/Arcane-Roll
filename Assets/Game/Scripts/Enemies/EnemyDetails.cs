using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetails : MonoBehaviour
{
    private EnemySO enemyData;
    private AbilitySO activeAbility;
    private List<Damageable> targets;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject imageGO;
    [SerializeField] private GameObject abilityGO;
    [SerializeField] private GameObject abilityDescriptionGO;
    [SerializeField] private GameObject abilityImageGO;
    [SerializeField] private TextMeshProUGUI abilityTitleText;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;

    [Header("Targets")]
    [SerializeField] private GameObject targetContainerGO;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject arrowIcon;

    public EnemySO EnemyData => enemyData;
    public List<Damageable> Targets => targets;

    private void Update()
    {
        abilityGO.SetActive(activeAbility != null);
    }

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

    public void SetAbilityDetails(int abilityPosition, List<Damageable> _targets = null)
    {
        if (abilityPosition == -1 || abilityPosition > enemyData.Abilities.Length)
        {
            activeAbility = null;
            return;
        }
        
        if (_targets != null)
        {
            arrowIcon.SetActive(true);
            arrowIcon.transform.localScale = Vector3.zero;
            arrowIcon.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            targets = _targets;
            foreach(Damageable target in targets)
            {
                GameObject got = Instantiate(targetPrefab, targetContainerGO.transform);
                got.GetComponent<TargetUI>().SetTarget(target);
                got.GetComponent<TargetUI>().AnimateIn();
            }
        }
        
        AbilitySO _ability = enemyData.Abilities[abilityPosition];
        abilityTitleText.text = _ability.abilityName;
        abilityDescriptionText.text = _ability.description;
        abilityImageGO.GetComponent<Image>().sprite = _ability.icon;
        activeAbility = _ability;
    }

    public void ToggleAbilityDescription()
    {
        IBattleState currState = GameManager.Instance.GetComponent<BattleManager>().GetState();
        
        if (currState is not EnemyPlanningPhase)
        {
            abilityDescriptionGO.SetActive(!abilityDescriptionGO.activeInHierarchy);
        }
    }
    public void ShowAbilityDescription()
    {
        abilityDescriptionGO.SetActive(true);
    }
    public void HideAbilityDescription()
    {
        abilityDescriptionGO.SetActive(false);
    }
}
