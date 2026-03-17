using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    public static void SetPaused(bool pause)
    {
        IsGamePaused = pause;
    }
}
