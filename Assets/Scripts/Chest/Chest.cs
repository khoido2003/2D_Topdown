using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }

    public string chestID { get; private set; }

    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private Sprite openedSprite;

    private void Awake()
    {
        chestID = GlobalHelpers.GenerateUniqueId(gameObject);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(
                itemPrefab,
                transform.position + Vector3.down,
                Quaternion.identity
            );

            droppedItem.GetComponent<BoundEffect>().StartBounce();
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;

        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}
