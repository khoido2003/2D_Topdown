using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [SerializeField]
    private GameObject dialoguePanel;

    public TextMeshProUGUI dialogueText,
        nameText;

    [SerializeField]
    private Image portraitImage;

    [SerializeField]
    private Transform choicesPanel;

    [SerializeField]
    private GameObject choiceBtnPrefab;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.SetText(npcName);
        portraitImage.sprite = portrait;
    }

    public void SetDialogue(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateChoiceBtn(string choiceText, UnityAction onClick)
    {
        GameObject choiceBtn = Instantiate(choiceBtnPrefab, choicesPanel);

        choiceBtn.GetComponentInChildren<TextMeshProUGUI>().text = choiceText;

        choiceBtn.GetComponent<Button>().onClick.AddListener(onClick);

        return choiceBtn;
    }
}
