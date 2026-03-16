using UnityEngine;

public static class GlobalHelpers
{
    public static string GenerateUniqueId(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}";
    }
}
