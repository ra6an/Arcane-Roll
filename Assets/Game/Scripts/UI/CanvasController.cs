using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class CanvasController : MonoBehaviour
{
    [Foldout("Starter Set Panel")]
    [SerializeField] private GameObject starterSetPanel;

    [Foldout("Players Monster Panel")]
    [SerializeField] public GameObject playerMonstersPanel;

    [Foldout("Relics Panel")]
    [SerializeField] private GameObject relicsPanel;
    [SerializeField] public GameObject relicsContainerGO;

    [Foldout("Map Panel")]
    [SerializeField] public GameObject roomsMapPanel;
    [SerializeField] public RectTransform roomsMapRect;
    [SerializeField] public GameObject mapGO;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] public RectTransform roomPanelTransform;
    [SerializeField] public RectTransform linePanelTransform;

    [Foldout("Buttons UI Panel")]
    [SerializeField] public GameObject UIButtonsPanel;

    [Foldout("Loading Panel")]
    [SerializeField] public GameObject loadingPanel;
    [SerializeField] public RectTransform loadingContentRect;

    [Foldout("Enemy Team Panel")]
    [SerializeField] public GameObject enemyTeamPanel;
    [SerializeField] public RectTransform enemyTeamRect;

    [Foldout("Info Panel")]
    [SerializeField] public GameObject infoPanel;
    [SerializeField] public RectTransform infoRect;

    [Foldout("End Battle Panel")]
    [SerializeField] public GameObject endBattlePanel;
    [SerializeField] public RectTransform endBattleRect;

    [Foldout("State Change Panel")]
    [SerializeField] public GameObject stateChangePanel;
    [SerializeField] public RectTransform stateChangeMsgRect;
    //[SerializeField] public GameObject infoMessagePanel;
    //[SerializeField] public RectTransform infoMessageRect;

    public void CloseAllPanels()
    {
        starterSetPanel.SetActive(false);
        playerMonstersPanel.SetActive(false);
        relicsPanel.SetActive(false);
        roomsMapPanel.SetActive(false);
        UIButtonsPanel.SetActive(false);
    }

    public void ShowStarterSetPanel()
    {
        if (starterSetPanel == null) return;

        starterSetPanel.SetActive(true);
    }
    public void HideStarterSetPanel()
    {
        if(starterSetPanel == null) return;

        starterSetPanel.SetActive(false);
        GameManager.Instance.GetComponent<StarterSetsController>().RemoveStarterSets();
    }

    public void ShowPlayerMonstersPanel()
    {
        if (playerMonstersPanel == null) return;

        playerMonstersPanel.SetActive(true);
    }

    public void ShowRelicsPanel()
    {
        if (relicsPanel == null) return;
        relicsPanel.GetComponent<RelicsPanelAnimator>().ShowPanel();
    }
    public void HideRelicsPanel()
    {
        if (relicsPanel == null) return;
        relicsPanel.GetComponent<RelicsPanelAnimator>().HidePanel();
    }

    public void ShowUIButtons()
    {
        if (UIButtonsPanel == null) return;
        UIButtonsPanel.GetComponent<TopUIAnimator>().ShowTopUI();
    }
    public void HideUIButtons()
    {
        if (UIButtonsPanel == null) return;
        UIButtonsPanel.GetComponent<TopUIAnimator>().HideTopUI();
    }

    // BATTLE UI
    public void ShowBattleUI()
    {
        if (UIButtonsPanel == null || relicsPanel == null) return;

        UIButtonsPanel.GetComponent<TopUIAnimator>().ShowTopUI();
        relicsPanel.GetComponent<RelicsPanelAnimator>().ShowPanel();
        playerMonstersPanel.GetComponent<MonsterDetailsAnimator>().ShowPanel();


    }
    public void HideBattleUI()
    {
        if (UIButtonsPanel == null || relicsPanel == null) return;

        UIButtonsPanel.GetComponent<TopUIAnimator>().HideTopUI();
        relicsPanel.GetComponent<RelicsPanelAnimator>().HidePanel();
        playerMonstersPanel.GetComponent<MonsterDetailsAnimator>().HidePanel();
    }

    // MAP
    public void ShowRoomsMapPanel()
    {
        if(roomsMapPanel == null) return;
        roomsMapPanel.SetActive(true);
        roomsMapPanel.GetComponent<RoomsMapController>().ShowPanel();
    }
    public void HideRoomsMapPanel()
    {
        if (roomsMapPanel == null) return;
        roomsMapPanel.GetComponent<RoomsMapController>().HidePanel();
    }

    // LOADING UI
    public void ShowLoadingPanel()
    {
        if(loadingPanel == null) return;
        loadingPanel.GetComponent<LoadingPanelAnimator>().ShowPanel();
    }
    public void HideLoadingPanel()
    {
        if(loadingPanel == null) return;
        loadingPanel.GetComponent <LoadingPanelAnimator>().HidePanel();
    }

    // END BATTLE UI
    public void ShowEndBattlePanel(bool isVictory)
    {
        if(endBattlePanel == null) return;
        endBattlePanel.GetComponent<EndBattleController>().ShowEndBattlePanel(isVictory);
    }
}
