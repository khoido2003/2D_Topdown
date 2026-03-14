using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private CinemachineConfiner2D confiner;

    private string saveLocation;

    [SerializeField]
    private InventoryController inventoryController;

    private void Awake()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(saveLocation);

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPos = player.position,
            mapBoundary = confiner.BoundingShape2D.name,
            inventorySaveData = inventoryController.GetInventoryItem(),
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));

        Debug.Log("Saved!!!!");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame();
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        inventoryController.SetInventoryItem(saveData.inventorySaveData);

        player.position = saveData.playerPos;

        confiner.BoundingShape2D = GameObject
            .Find(saveData.mapBoundary)
            .GetComponent<PolygonCollider2D>();
    }
}
