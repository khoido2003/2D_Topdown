using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;

    public GameObject slotPrefab;

    public int slotCount;

    public GameObject[] itemPrefabs;

    private void Start()
    {
        /*
        for (int i = 0; i < slotCount; i++)
        {
            SlotUI slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<SlotUI>();

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);

                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                slot.currentItem = item;
            }
        }
        */
    }

    public bool TryAddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotUI slot = slotTransform.GetComponent<SlotUI>();

            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                slot.currentItem = newItem;

                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }

    public List<InventorySaveData> GetInventoryItem()
    {
        List<InventorySaveData> inventorySavesData = new();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotUI slot = slotTransform.GetComponent<SlotUI>();

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                inventorySavesData.Add(
                    new InventorySaveData
                    {
                        itemId = item.GetId(),
                        slotIndex = slotTransform.GetSiblingIndex(),
                    }
                );
            }
        }

        return inventorySavesData;
    }

    public void SetInventoryItem(List<InventorySaveData> inventorySaveDatas)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveDatas)
        {
            if (data.slotIndex < slotCount)
            {
                SlotUI slot = inventoryPanel
                    .transform.GetChild(data.slotIndex)
                    .GetComponent<SlotUI>();

                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemId);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);

                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    slot.currentItem = item;
                }
            }
        }
    }
}
