using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIDragHandler
    : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerClickHandler
{
    private InventoryController inventoryController;

    private Transform originSlotParent;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float minDropDistance = 2f;

    [SerializeField]
    private float maxDropDistance = 3f;

    private void Start()
    {
        inventoryController = InventoryController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originSlotParent = transform.parent;

        transform.SetParent(transform.root);

        canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        SlotUI dropSlot = eventData.pointerEnter?.GetComponent<SlotUI>();

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;

            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<SlotUI>();
            }
        }

        SlotUI originalSlot = originSlotParent.GetComponent<SlotUI>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                if (draggedItem.GetId() == targetItem.GetId())
                {
                    targetItem.AddToToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    // SLot has item -> Swap them
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;

                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition =
                        Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;

                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
        else
        {
            // If dropping not within the inventory => drop the item to the ground

            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                // else snap back in UI
                transform.SetParent(originSlotParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    private bool IsWithinInventory(Vector2 mousePos)
    {
        RectTransform inventoryRect = originSlotParent.parent.GetComponent<RectTransform>();

        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePos);
    }

    private void DropItem(SlotUI originalSlot)
    {
        Item item = GetComponent<Item>();
        int quantity = item.quantity;

        if (quantity > 1)
        {
            item.RemoveFromStack();
            transform.SetParent(originSlotParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            quantity = 1;
        }
        else
        {
            originalSlot.currentItem = null;
        }

        // Find Player
        Transform playerTf = GameObject.FindWithTag("Player")?.transform;

        if (playerTf == null)
        {
            Debug.LogError("No player found!");

            return;
        }

        // drop to position
        Vector2 dropOffset =
            Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPos = (Vector2)playerTf.position + dropOffset;

        // Create the object
        GameObject dropItem = Instantiate(gameObject, dropPos, Quaternion.identity);

        Item droppedItem = dropItem.GetComponent<Item>();
        droppedItem.quantity = 1;

        dropItem.GetComponent<BoundEffect>().StartBounce();

        if (quantity <= 1 && originalSlot.currentItem == null)
        {
            //Destroy the UI

            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }
    }

    private void SplitStack()
    {
        Item item = GetComponent<Item>();

        if (item == null || item.quantity <= 1)
        {
            return;
        }

        int splitAmount = item.quantity / 2;
        if (splitAmount <= 0)
        {
            return;
        }

        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneObject(splitAmount);

        if (inventoryController == null || newItem == null)
        {
            return;
        }

        foreach (Transform slotTf in inventoryController.inventoryPanel.transform)
        {
            SlotUI slot = slotTf.GetComponent<SlotUI>();

            if (slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                return;
            }
        }

        item.AddToToStack(splitAmount);

        Destroy(newItem);
    }
}
