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

        for(int i = 0; i < deck.Cards.Length; i++)
        {
            CardSO currentCard = deck.Cards[i];

            // INSTANTIATE 3D CRYSTAL AND SETUP SCRIPT FOR MONSTER
            GameObject go = Instantiate(teamMonsterPrefab, teamContainer.transform);
            go.GetComponent<AllyCrystalController>().SetMonster(currentCard, i);
            CrystalsContainer cc = _crystalsContainerGO.GetComponent<CrystalsContainer>();
            Vector3 availablePos = cc.crystalsPositions[i];
            Vector3 ccPos = cc.transform.position;
            Vector3 position = new(ccPos.x + availablePos.x, ccPos.y + availablePos.y, ccPos.z + availablePos.z);
            go.transform.position = position;
            go.SetActive(false);

            // INSTANTIATE AND SETUP UI FOR MONSTER
            CanvasController _canvas = GameManager.Instance.Canvas.GetComponent<CanvasController>();
            _canvas.playerMonstersPanel.GetComponent<PlayerMonstersController>().AddPlayerMonster(currentCard, i);
        }
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
            //Debug.Log(ally.name);
            Damageable da = ally.GetComponent<Damageable>();
            //Debug.Log($"PRIJE IFA!!! {da != null} {da.IsAlive()}");
            if(da != null && da.IsAlive())
            {
                Debug.Log("U IFU!!!");
                listToReturn.Add(da);
            }
            //Debug.Log("POSLIJE IFA!!!");
        }

        return listToReturn;
    }
}
