using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionIcon;

    private IInteractable interactableInRange = null;

    private void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputValue input)
    {
        if (input.isPressed)
        {
            Debug.Log("Open chest");
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable == interactableInRange)
            {
                interactableInRange = null;
                interactionIcon.SetActive(false);
            }
        }
    }
}
