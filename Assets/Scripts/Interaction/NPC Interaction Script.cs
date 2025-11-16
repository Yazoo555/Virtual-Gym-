using System.Collections; // Add this line
using UnityEngine;
 

public class InteractableNPC : InteractableBase
{
    [Header("NPC Specific Settings")]
    [SerializeField] private float typingSpeed = 0.05f;

    public override void TriggerInteraction()
    {
        ShowInteractionPrompt(false);
        if (dialoguePanel == null || textComponent == null) return;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        CancelInvoke(nameof(HideDialogue));
        dialoguePanel.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(dialogueText));
    }

    private IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char c in textToType)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        Invoke(nameof(HideDialogue), displayDuration);
    }

    public override void SkipInteraction()
    {
        if (!isTyping) return;

        StopCoroutine(typingCoroutine);
        textComponent.text = dialogueText;
        isTyping = false;
        CancelInvoke(nameof(HideDialogue));
        Invoke(nameof(HideDialogue), displayDuration);
    }
}