using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField]
    private InventoryController inventoryController;

    private void Start() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();

            if (item != null)
            {
                bool isItemAdded = inventoryController.TryAddItem(collision.gameObject);

                if (isItemAdded)
                {
                    item.Pickup();
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
