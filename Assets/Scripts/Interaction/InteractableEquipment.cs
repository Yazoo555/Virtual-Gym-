// InteractableEquipment.cs
public class InteractableEquipment : InteractableBase
{
    public override void TriggerInteraction()
    {
        ShowInteractionPrompt(false);
        if (dialoguePanel == null || textComponent == null) return;

        CancelInvoke(nameof(HideDialogue));
        dialoguePanel.SetActive(true);
        textComponent.text = dialogueText; // Instant text display
        Invoke(nameof(HideDialogue), displayDuration);
    }

    public override void SkipInteraction()
    {
        CancelInvoke(nameof(HideDialogue));
        HideDialogue();
    }
}