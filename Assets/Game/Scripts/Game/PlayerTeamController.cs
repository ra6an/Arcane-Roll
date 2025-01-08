using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTeamController : MonoBehaviour
{
    private GameObject _crystalsContainerGO;
    [SerializeField] private GameObject teamContainer;
    [SerializeField] private GameObject teamMonsterPrefab;

    private void Awake()
    {
        _crystalsContainerGO = GameObject.FindWithTag("CrystalsSpawnPoints");
    }

    public void SetAllyTeam()
    {
        DeckSO deck = PlayerController.Instance.CurrentGameState.deck;
        CleanTeam();
        for (int i = 0; i < deck.Cards.Length; i++)
        {
            CardSO currentCard = deck.Cards[i];

            // INSTANTIATE 3D CRYSTAL AND SETUP SCRIPT FOR MONSTER
            GameObject go = Instantiate(teamMonsterPrefab, teamContainer.transform);
            AllyCrystalController acc = go.GetComponent<AllyCrystalController>();
            acc.SetMonster(currentCard, i);
            
            CrystalsContainer cc = _crystalsContainerGO.GetComponent<CrystalsContainer>();
            Vector3 availablePos = cc.crystalsPositions[i];
            Vector3 ccPos = cc.transform.position;
            Vector3 position = new(ccPos.x + availablePos.x, ccPos.y + availablePos.y, ccPos.z + availablePos.z);
            go.transform.position = position;
            go.SetActive(false);

            // COMBINE CRYSTAL AND DICE ROLL MACHINE
            DiceManager.Instance.SetCrystalInStates(acc);

            // INSTANTIATE AND SETUP UI FOR MONSTER
            CanvasController _canvas = GameManager.Instance.Canvas.GetComponent<CanvasController>();
            _canvas.playerMonstersPanel.GetComponent<PlayerMonstersController>().AddPlayerMonster(currentCard, i);

        }
    }

    private void CleanTeam()
    {
        int childCount = teamContainer.transform.childCount;
        if (childCount == 0) return;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = teamContainer.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void ClearCrystalsPrefabs()
    {
        CleanTeam();
    }

    public void MaterializeCrystals()
    {
        foreach(Transform crystal in teamContainer.transform)
        {
            // MATERIALIZE ALL CRYSTALS
            crystal.gameObject.SetActive(true);
            AllyCrystalController acc = crystal.GetComponent<AllyCrystalController>();
            acc.Materialize();
        }
    }

    public List<Damageable> GetAllyMonstersDamageable()
    {
        List<Damageable> listToReturn = new();

        foreach (Transform ally in teamContainer.transform)
        {
            Damageable da = ally.GetComponent<Damageable>();
            if(da != null && da.IsAlive())
            {
                listToReturn.Add(da);
            }
        }

        return listToReturn;
    }

    public void SetMonsterRolledDice(CardSO _card, int _rolledDice)
    {
        foreach(Transform ally in teamContainer.transform)
        {
            AllyCrystalController acc = ally.GetComponent<AllyCrystalController>();
            if (acc == null || acc.CardData != _card) continue;

            acc.SetRolledDice(_rolledDice);
        }
    }
}
