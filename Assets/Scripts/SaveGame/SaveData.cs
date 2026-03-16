using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public string mapBoundary;

    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    public List<ChessSaveData> chessSaveDatas;
}

[Serializable]
public class ChessSaveData
{
    public string ChessId;
    public bool isOpened;
}
