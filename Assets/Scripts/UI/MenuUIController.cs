using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuCanvas;

    private void Awake()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
