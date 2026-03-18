using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private NpcDialogue dialogueData;

    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI dialogueText,
        nameText;

    [SerializeField]
    private Image portraitImage;

    private int dialogueIndex;

    private bool isTyping,
        isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseGame.IsGamePaused && !isDialogueActive))
        {
            return;
        }

        if (isDialogueActive)
        {
            // Next Line
            NextLine();
        }
        else
        {
            // Start Dialogue
            StartDialogue();
        }
    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseGame.SetPaused(true);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = false;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;

            SoundEffectManager.Instance.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        if (
            dialogueData.autoProgressLines.Length > dialogueIndex
            && dialogueData.autoProgressLines[dialogueIndex]
        )
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseGame.SetPaused(false);
    }
}
