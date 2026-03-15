using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originSlotParent;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float minDropDistance = 2f;

    [SerializeField]
    private float maxDropDistance = 3f;

    private void Start()
    {
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
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;

                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
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
            }
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private bool IsWithinInventory(Vector2 mousePos)
    {
        RectTransform inventoryRect = originSlotParent.parent.GetComponent<RectTransform>();

        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePos);
    }

    private void DropItem(SlotUI originalSlot)
    {
        originalSlot.currentItem = null;

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

        dropItem.GetComponent<BoundEffect>().StartBounce();

        //Destroy the UI
        Destroy(gameObject);
    }
}
