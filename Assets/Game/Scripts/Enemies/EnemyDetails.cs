using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetails : MonoBehaviour
{
    private PlayerMonstersController _playerMonstersController;

    private int spawnId;
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

    [Header("Checkbox")]
    [SerializeField] private GameObject checkBoxGO;
    [SerializeField] private GameObject checkboxIconGO;

    public int SpawnId => spawnId;
    public EnemySO EnemyData => enemyData;
    public List<Damageable> Targets => targets;

    private void Awake()
    {
        CanvasController _c = GameManager.Instance.Canvas.GetComponent<CanvasController>();
        if (_c != null)
        {
            _playerMonstersController = _c.playerMonstersPanel.GetComponent<PlayerMonstersController>();
        }
    }

    private void Update()
    {
        abilityGO.SetActive(activeAbility != null);
        HandleShowCheckBox();
    }

    public void SetEnemyDetails(EnemySO _data, int _spawnId)
    {
        if (_data == null) return;

        spawnId = _spawnId;
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

    public void ShowCheckBox()
    {
        checkboxIconGO.SetActive(false);
        checkBoxGO.SetActive(true);
    }
    public void HideCheckBox()
    {
        checkboxIconGO.SetActive(false);
        checkBoxGO.SetActive(false);
    }

    private void HandleShowCheckBox()
    {
        if (_playerMonstersController == null)
        {
            CanvasController _c = GameManager.Instance.Canvas.GetComponent<CanvasController>();
            if (_c != null)
            {
                _playerMonstersController = _c.playerMonstersPanel.GetComponent<PlayerMonstersController>();
            }
        }

        if (_playerMonstersController.ActivatedAbility.monster != null && _playerMonstersController.ActivatedAbility.monster.NeedEnemyTargets > 0)
        {
            checkBoxGO.SetActive(true);
            List<Damageable> selectedTargets = _playerMonstersController.ActivatedAbility.enemyTargets;
            EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
            Damageable d = ec.GetEnemyDamageableBySpawnId(spawnId);

            if(d != null)
            {
                if (selectedTargets.Contains(d))
                {
                    checkboxIconGO.SetActive(true);
                }
                else
                {
                    checkboxIconGO.SetActive(false);
                }
            }
        }
        else
        {
            checkBoxGO.SetActive(false);
        }
    }

    public void OnCheckBoxClicked()
    {
        if (_playerMonstersController.ActivatedAbility.monster.GetRemainingEnemyTargetSpots() > 0)
        {
            EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
            Damageable d = ec.GetEnemyDamageableBySpawnId(spawnId);
            
            if( d != null )
            {
                if (_playerMonstersController.ActivatedAbility.enemyTargets.Contains(d)) return;
                _playerMonstersController.ActivatedAbility.monster.SetDiceRollStateEnemyTarget(d);
                _playerMonstersController.AddEnemyTarget(d);
            }
        }
    }
}
