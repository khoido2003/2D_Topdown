using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject hotbarPanel;

    [SerializeField]
    private GameObject slotPrefab;

    [SerializeField]
    private int slotCount = 10;

    private Key[] hotbarKeys;

    [SerializeField]
    private ItemDictionary itemDictionary;

    private void Awake()
    {
        hotbarKeys = new Key[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    private void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    private void UseItemInSlot(int index)
    {
        SlotUI slot = hotbarPanel.transform.GetChild(index).GetComponent<SlotUI>();

        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();

            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotbarItem()
    {
        List<InventorySaveData> inventorySavesData = new();

        foreach (Transform slotTransform in hotbarPanel.transform)
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

    public void SetHotbarItem(List<InventorySaveData> inventorySaveDatas)
    {
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveDatas)
        {
            if (data.slotIndex < slotCount)
            {
                SlotUI slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<SlotUI>();

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
