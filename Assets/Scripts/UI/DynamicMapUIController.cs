using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicMapUIController : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField]
    private RectTransform mapParent;

    [SerializeField]
    private GameObject areaPrefab;

    [SerializeField]
    private RectTransform playerIcon;

    [Header("Icon")]
    [SerializeField]
    private Color defaultColor = Color.gray;

    [SerializeField]
    private Color currentColor = Color.green;

    [Header("Map settings")]
    [SerializeField]
    private GameObject mapBounds;

    [SerializeField]
    private PolygonCollider2D initArea;

    [SerializeField]
    private float mapScale = 10f;

    private PolygonCollider2D[] mapAreas;

    private Dictionary<string, RectTransform> uiAreas = new();

    public static DynamicMapUIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mapAreas = mapBounds.GetComponentsInChildren<PolygonCollider2D>();
    }

    public void GenerateMap(PolygonCollider2D newCurrentArea = null)
    {
        PolygonCollider2D currentArea = newCurrentArea ?? initArea;

        ClearMap();

        foreach (PolygonCollider2D area in mapAreas)
        {
            CreateAreaUI(area, area == currentArea);
        }

        MovePlayerIcon(currentArea.name);
    }

    private void ClearMap()
    {
        foreach (Transform child in mapParent)
        {
            Destroy(child.gameObject);
        }

        uiAreas.Clear();
    }

    private void CreateAreaUI(PolygonCollider2D area, bool isCurrent)
    {
        GameObject areaImage = Instantiate(areaPrefab, mapParent);

        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();

        Bounds bound = area.bounds;

        rectTransform.sizeDelta = new Vector2(bound.size.x * mapScale, bound.size.y * mapScale);
        rectTransform.anchoredPosition = bound.center * mapScale;

        areaImage.GetComponent<Image>().color = isCurrent ? currentColor : defaultColor;

        uiAreas[area.name] = rectTransform;
    }

    public void UpdateCurrentArea(string newCurrentArea)
    {
        foreach (KeyValuePair<string, RectTransform> area in uiAreas)
        {
            area.Value.GetComponent<Image>().color =
                area.Key == newCurrentArea ? currentColor : defaultColor;
        }

        MovePlayerIcon(newCurrentArea);
    }

    private void MovePlayerIcon(string newCurrentArea)
    {
        if (uiAreas.TryGetValue(newCurrentArea, out RectTransform areaUI))
        {
            playerIcon.anchoredPosition = areaUI.anchoredPosition;
        }
    }
}
