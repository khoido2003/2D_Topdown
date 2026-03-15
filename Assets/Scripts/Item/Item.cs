using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int ID;

    public string Name;

    public int GetId() => ID;

    public void SetId(int id)
    {
        ID = id;
    }

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Using item " + Name);
    }
}
