using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private ItemDictionary itemDictionary;

    public static InventoryController Instance { get; private set; }

    public GameObject inventoryPanel;

    public GameObject slotPrefab;

    public int slotCount;

    public GameObject[] itemPrefabs;

    private void Awake()
    {
        Instance = this;
    }

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
        Item itemToAdd = itemPrefab.GetComponent<Item>();

        if (itemToAdd == null)
        {
            return false;
        }

        // Check if this item already inside the inventory
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotUI slot = slotTransform.GetComponent<SlotUI>();

            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();

                if (slotItem != null && slotItem.GetId() == itemToAdd.GetId())
                {
                    slotItem.AddToToStack();
                    return true;
                }
            }
        }

        // If not, look for empty slot to add that new item
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
                        quantity = item.quantity,
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

                    Item itemComponent = item.GetComponent<Item>();
                    if (itemComponent != null && data.quantity > 1)
                    {
                        itemComponent.quantity = data.quantity;
                        itemComponent.UpdateQuantityText();
                    }

                    slot.currentItem = item;
                }
            }
        }
    }
}
