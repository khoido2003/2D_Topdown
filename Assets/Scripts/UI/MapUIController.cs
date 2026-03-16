using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapUIController : MonoBehaviour
{
    public static MapUIController Instance { get; private set; }

    public GameObject mapParent;

    private List<Image> mapImages;

    [SerializeField]
    private Color highlightColor = Color.yellow;

    [SerializeField]
    private Color dimmedColor = new Color(1f, 1f, 1f, 0.5f);

    [SerializeField]
    private RectTransform playerIconTf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        mapImages = mapParent.GetComponentsInChildren<Image>().ToList();
    }

    public void HighlightArea(string areaName)
    {
        foreach (Image area in mapImages)
        {
            area.color = dimmedColor;
        }

        Image currentArea = mapImages.Find(x => x.name == areaName);

        if (currentArea != null)
        {
            currentArea.color = highlightColor;
            Vector3 position = currentArea.GetComponent<RectTransform>().position;

            playerIconTf.position = position;
        }
        else
        {
            Debug.LogError("No area found! Check the name" + areaName);
        }
    }
}
