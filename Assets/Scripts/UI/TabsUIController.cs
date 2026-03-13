using UnityEngine;
using UnityEngine.UI;

public class TabsUIController : MonoBehaviour
{
    [SerializeField]
    private Image[] tabsImages;

    [SerializeField]
    private GameObject[] pages;

    private void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabsImages[i].color = Color.gray;
        }

        pages[tabNo].SetActive(true);
        tabsImages[tabNo].color = Color.white;
    }
}
