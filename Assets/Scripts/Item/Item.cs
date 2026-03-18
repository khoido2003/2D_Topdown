using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int ID;

    public string Name;

    public TextMeshProUGUI quantityText;

    public int quantity = 1;

    public int GetId() => ID;

    private void Awake() { }

    private void Start()
    {
        UpdateQuantityText();
    }

    public void UpdateQuantityText()
    {
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
    }

    public void AddToToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityText();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);

        quantity -= removed;

        UpdateQuantityText();

        return removed;
    }

    public GameObject CloneObject(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();

        cloneItem.quantity = newQuantity;
        cloneItem.UpdateQuantityText();

        return clone;
    }

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
