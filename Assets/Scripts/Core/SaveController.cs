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
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame();
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        player.position = saveData.playerPos;

        confiner.BoundingShape2D = GameObject
            .Find(saveData.mapBoundary)
            .GetComponent<PolygonCollider2D>();
    }
}
