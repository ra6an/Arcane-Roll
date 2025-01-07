using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum InfoMessageType
{
    Turn,
    AbilityActivation,
    BattlePhaseChanged,
}

public class InfoMessageContext
{
    public int id;
    public InfoMessageType type;
    public string text;
    public bool isShown;

    public InfoMessageContext()
    {
        isShown = false;
    }
}

public class InfoPanelController : MonoBehaviour
{
    [SerializeField] private GameObject infoMessageGO;
    [SerializeField] private TextMeshProUGUI infoMessageText;
    private List<InfoMessageContext> infoLog = new();
    private int currentShowingMsg = -1;
    private bool showingInfoMsg = false;

    private void Update()
    {
        if(!showingInfoMsg)
        {
            CheckAndShowInfo();
        }
    }

    public void AddNewLog(InfoMessageContext context)
    {
        if (context == null) return;
        InfoMessageContext _iModified = context;
        _iModified.id = infoLog.Count;
        infoLog.Add(context);
    }

    private void ShowMessage(InfoMessageContext mc)
    {
        showingInfoMsg = true;
        infoMessageGO.SetActive(true);
        infoMessageText.text = mc.text;
        StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2);
        infoMessageGO.SetActive(false);
        infoMessageText.text = "";
        StartCoroutine(AllowShowingNextMessage());
    }

    private IEnumerator AllowShowingNextMessage()
    {
        yield return new WaitForSeconds(.8f);
        infoLog[currentShowingMsg].isShown = true;
        currentShowingMsg = -1;
        showingInfoMsg = false;
    }

    private void CheckAndShowInfo()
    {
        if (showingInfoMsg) return;

        bool haveMsgToShow = CheckInfoNotShown();

        if (haveMsgToShow && currentShowingMsg != -1)
        {
            ShowMessage(infoLog[currentShowingMsg]);
        }
    }

    private bool CheckInfoNotShown()
    {
        bool exist = false;

        foreach (InfoMessageContext context in infoLog)
        {
            if (context != null && !context.isShown)
            {
                exist = true;
                currentShowingMsg = infoLog.IndexOf(context);
                break;
            }
        }

        return exist;
    }
}
