using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [SerializeField]
    private HotbarUIController hotbarController;

    private Chest[] chests;

    private void Awake()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(saveLocation);
    }

    private void Start()
    {
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPos = player.position,
            mapBoundary = confiner.BoundingShape2D.name,
            inventorySaveData = inventoryController.GetInventoryItem(),
            hotbarSaveData = hotbarController.GetHotbarItem(),
            chessSaveDatas = GetChestsState(),
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));

        Debug.Log("Saved!!!!");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame();

            DynamicMapUIController.Instance?.GenerateMap();
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        inventoryController.SetInventoryItem(saveData.inventorySaveData);
        hotbarController.SetHotbarItem(saveData.hotbarSaveData);

        player.position = saveData.playerPos;

        PolygonCollider2D savedBoundary = GameObject
            .Find(saveData.mapBoundary)
            .GetComponent<PolygonCollider2D>();

        confiner.BoundingShape2D = savedBoundary;

        //MapUIController.Instance?.HighlightArea(saveData.mapBoundary);
        DynamicMapUIController.Instance?.GenerateMap(savedBoundary);

        // Load chest state
        LoadChestsState(saveData.chessSaveDatas);
    }

    private List<ChessSaveData> GetChestsState()
    {
        List<ChessSaveData> chestsState = new();

        foreach (Chest chest in chests)
        {
            ChessSaveData chessSaveData = new ChessSaveData
            {
                ChessId = chest.chestID,
                isOpened = chest.IsOpened,
            };

            chestsState.Add(chessSaveData);
        }
        return chestsState;
    }

    private void LoadChestsState(List<ChessSaveData> chessSaveDatas)
    {
        foreach (Chest chest in chests)
        {
            ChessSaveData chessSaveData = chessSaveDatas.FirstOrDefault(c =>
                c.ChessId == chest.chestID
            );

            if (chessSaveData != null)
            {
                chest.SetOpened(chessSaveData.isOpened);
            }
        }
    }
}
