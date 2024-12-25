using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType
{
    BATTLE,
    TREASURE,
    STORE,
    BOSS
}

public class RoomsMapController : MonoBehaviour
{
    private CanvasController _canvasController;
    private CanvasGroup roomsMapCanvasGroup;
    private MapSettings _mapSettings;

    private void Awake()
    {
        GameObject _canvas = GameObject.Find("Canvas");
        _canvasController = _canvas.GetComponent<CanvasController>();
        _mapSettings = GameManager.Instance.MapSettings;

        roomsMapCanvasGroup = _canvasController.roomsMapRect.GetComponent<CanvasGroup>();

        if(roomsMapCanvasGroup == null)
        {
            roomsMapCanvasGroup = _canvasController.roomsMapRect.gameObject.AddComponent<CanvasGroup>();
        }

        roomsMapCanvasGroup.alpha = 0f;
    }

    public void SetupMap()
    {
        SetMapSize();

        GenerateUI(GameManager.Instance.Map);

        ScrollToBottom();
    }

    public void ShowPanel()
    {
        SetupMap();
        this.GetComponent<RoomsMapAnimator>().ShowPanel();
    }

    public void HidePanel()
    {
        this.GetComponent<RoomsMapAnimator>().HidePanel(this.gameObject);
    }

    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        _canvasController.scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private void SetMapSize()
    {
        MapSettings ms = GameManager.Instance.MapSettings;

        int mapHeight = (ms.Layers * ms.YSpacing) + 250;

        if (_canvasController.mapGO != null)
        {
            RectTransform rt = _canvasController.mapGO.GetComponent<RectTransform>();
            Vector2 newSize = rt.sizeDelta;
            newSize.y = mapHeight;
            rt.sizeDelta = newSize;
        }
    }

    public void GenerateUI(List<List<Node>> map)
    {
        ClearRooms();
        ClearConnects();

        GameObject roomPrefab = _mapSettings.RoomPrefab;

        for (int i = 0; i < map.Count; i++)
        {
            var layer = map[i];
            foreach (var node in layer)
            {
                GameObject roomUI = Instantiate(roomPrefab, _canvasController.roomPanelTransform);
                roomUI.GetComponent<RectTransform>().anchoredPosition = GetRoomPosition(node.LayerIndex, node.NodeIndex, layer.Count);

                var roomTypeComponent = roomUI.GetComponent<Room>();
                roomTypeComponent.SetRoom(node);

                foreach (var connectedNode in node.Connections)
                {
                    ConnectRooms(node, connectedNode, layer.Count, map[i + 1].Count);
                }

                roomTypeComponent.SetPickedPath(node.PickedPath);
            }
        }
    }

    public void ConnectRooms(Node fromNode, Node toNode, int totalNodesInLayer, int totalNodesInNextLayer)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(_canvasController.linePanelTransform, false); // Postavi kao dete canvasa

        // Dodaj Image komponentu za liniju
        Image lineImage = line.AddComponent<Image>();
        lineImage.sprite = _mapSettings.LineSprite;
        SetLineColor(toNode, lineImage);

        // Postavi pivot
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        BottomLeft(rectTransform);

        // Prona?i pozicije soba
        Vector2 startPos = GetRoomPosition(fromNode.LayerIndex, fromNode.NodeIndex, totalNodesInLayer);
        Vector2 endPos = GetRoomPosition(toNode.LayerIndex, toNode.NodeIndex, totalNodesInNextLayer);

        // Izra?unaj dužinu i ugao linije
        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude * 0.6f;

        // Postavi veli?inu i rotaciju linije
        rectTransform.sizeDelta = new Vector2(distance, _mapSettings.LineWidth); // Dužina linije, širina fiksna
        rectTransform.anchoredPosition = startPos + direction / 2; // Sredi liniju izme?u ta?aka
        rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void ClearConnects()
    {
        foreach (Transform child in _canvasController.linePanelTransform) Destroy(child.gameObject);
    }

    private void ClearRooms()
    {
        foreach (Transform child in _canvasController.roomPanelTransform) Destroy(child.gameObject);
    }

    public Vector2 GetRoomPosition(int layerIndex, int nodeIndex, int totalNodesInLayer)
    {
        float xSpacing = _mapSettings.XSpacing; // Horizontalni razmak izme?u soba
        float ySpacing = _mapSettings.YSpacing; // Vertikalni razmak izme?u slojeva (pozitivan jer idemo gore)
        float bottomPadding = _mapSettings.BottomPadding;

        // Širina panela
        float canvasWidth = _canvasController.roomPanelTransform.gameObject.GetComponent<RectTransform>().rect.width;

        // Centriranje soba na osnovu širine panela
        float centerX = canvasWidth / 2f;
        float x = centerX + (nodeIndex - (totalNodesInLayer - 1) / 2f) * xSpacing;

        // Pozicioniranje slojeva prema gore
        float y = bottomPadding + layerIndex * ySpacing;

        return new Vector2(x, y);
    }

    private void SetLineColor(Node _node, Image _img)
    {
        Color _colorToSet = Color.white;

        if (_node.Type == RoomType.BATTLE) _colorToSet = _mapSettings.BattleColor;
        if (_node.Type == RoomType.STORE) _colorToSet = _mapSettings.StoreColor;
        if (_node.Type == RoomType.TREASURE) _colorToSet = _mapSettings.TreasureColor;
        if (_node.Type == RoomType.BOSS) _colorToSet = _mapSettings.BossColor;

        _img.color = _colorToSet;
    }

    private void BottomLeft(RectTransform uiObject)
    {
        uiObject.anchorMin = new Vector2(0, 0);
        uiObject.anchorMax = new Vector2(0, 0);
        uiObject.pivot = new Vector2(0.5f, 0.5f);
    }
}
