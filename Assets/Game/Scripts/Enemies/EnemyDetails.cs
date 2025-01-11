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
    private int currentHealth;
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

    [Header("Health")]
    [SerializeField] private GameObject currentHealthSliderGO;

    public int SpawnId => spawnId;
    public EnemySO EnemyData => enemyData;
    public AbilitySO ActiveAbility => activeAbility;
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
        //abilityGO.SetActive(activeAbility != null);
        HideAbilityIfDead();
        HandleShowCheckBox();
        UpdateHealth();
    }

    private void SetHealth(int _newHealth)
    {
        if (true)
        {
            int newHealth = _newHealth;
            EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
            Damageable eDmg = ec.GetEnemyDamageableBySpawnId(spawnId);
            int maxHealth = eDmg.MaxHealth;

            if (newHealth > maxHealth) newHealth = maxHealth;

            //healthText.text = $"{newHealth} / {maxHealth}";
            currentHealth = newHealth;

            //Postaviti health bar

            currentHealthSliderGO.GetComponent<Image>().fillAmount = newHealth / (float)maxHealth;
        }
    }

    private void UpdateHealth()
    {
        //Damageable dmg = diceRollState.Crystal.GetComponent<Damageable>();
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        Damageable eDmg = ec.GetEnemyDamageableBySpawnId(spawnId);

        if (eDmg != null)
        {
            int enemyHealth = eDmg.GetHealth();
            if (currentHealth != enemyHealth)
            {
                SetHealth(enemyHealth);
            }
        }
    }

    public void SetEnemyDetails(EnemySO _data, int _spawnId)
    {
        if (_data == null) return;

        spawnId = _spawnId;
        enemyData = _data;
        currentHealth = _data.health;

        if(!string.IsNullOrEmpty(_data.enemyName))
        {
            titleText.text = _data.enemyName;
            imageGO.GetComponent<Image>().sprite = _data.sprite;
        }

    }

    private void HideAbilityIfDead()
    {
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        Damageable enemy = ec.GetEnemyDamageableBySpawnId(SpawnId);

        if(!enemy.IsAlive())
        {
            ClearTargetsContainer();
            abilityGO.SetActive(false);
        } else
        {
            abilityGO.SetActive(activeAbility != null);
        }

    }

    public void SetAbilityDetails(int abilityPosition, List<Damageable> _targets = null)
    {
        if (this.transform == null) return;
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
            ClearTargetsContainer();
            foreach (Damageable target in targets)
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

    private void ClearTargetsContainer()
    {
        foreach (Transform t in targetContainerGO.transform)
        {
            if (t == null) continue;
            DOTween.Kill(t.transform);
            Destroy(t.gameObject);
        }
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

    public bool IsAlive()
    {
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        Damageable eDmg = ec.GetEnemyDamageableBySpawnId(spawnId);
        bool isAlive = eDmg.IsAlive();

        return isAlive;
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
            if (!IsAlive()) return;
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

    public void ActivateAbility()
    {
        // Aktivacija ability-a
        Debug.Log("Abiliti aktiviran");
        EnemiesController ec = GameManager.Instance.GetComponent<EnemiesController>();
        Damageable d = ec.GetEnemyDamageableBySpawnId(spawnId);
        EnemyController enemyController = d.GetComponent<EnemyController>();
        enemyController.ActivateAbility(activeAbility, targets);
        // Animacije monster napada

        // applianje dmga/buffa/debuffa/etc

        // javljanje EnemyBattlePhaseu da je ability uspjesno aktiviran i zavrsen
        StartCoroutine(FinnishAbility());
    }

    private IEnumerator FinnishAbility()
    {
        yield return new WaitForSeconds(4);
        IBattleState currState = GameManager.Instance.GetComponent<BattleManager>().GetState();
        if (currState is EnemyBattlePhase ebp)
        {
            ebp.AbilityFinished();
            activeAbility = null;
            targets.Clear();
        }
    }
}
