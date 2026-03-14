using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    [SerializeField]
    private List<Item> itemPrefabs;

    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            itemPrefabs[i].SetId(i + 1);
        }

        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.GetId()] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemId)
    {
        itemDictionary.TryGetValue(itemId, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemId} not found in dictionary");
        }

        return prefab;
    }
}
