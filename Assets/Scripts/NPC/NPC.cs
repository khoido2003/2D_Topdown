using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    private int dialogueIndex;

    [SerializeField]
    private NpcDialogue dialogueData;

    private DialogueController dialogueController;

    private bool isTyping,
        isDialogueActive;

    private void Start()
    {
        dialogueController = DialogueController.Instance;
    }

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
            dialogueController.SetDialogue(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        // Clear choices
        dialogueController.ClearChoices();

        if (
            dialogueData.endProgressLines.Length > dialogueIndex
            && dialogueData.endProgressLines[dialogueIndex]
        )
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                //
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
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

        dialogueController.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueController.ShowDialogueUI(true);

        PauseGame.SetPaused(true);

        DisplayCurrentLine();
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        dialogueController.SetDialogue("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueController.SetDialogue(dialogueController.dialogueText.text + letter);

            SoundEffectManager.Instance.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                yield break; // stop here, wait for player choice
            }
        }

        if (
            dialogueData.autoProgressLines.Length > dialogueIndex
            && dialogueData.autoProgressLines[dialogueIndex]
        )
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    private void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];

            dialogueController.CreateChoiceBtn(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueController.ClearChoices();

        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueController.SetDialogue("");
        dialogueController.ShowDialogueUI(false);

        PauseGame.SetPaused(false);
    }
}
