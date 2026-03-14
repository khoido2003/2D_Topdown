using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int ID;

    public int GetId() => ID;

    public void SetId(int id)
    {
        ID = id;
    }
}
