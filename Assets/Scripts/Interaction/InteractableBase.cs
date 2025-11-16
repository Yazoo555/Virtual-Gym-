// InteractableBase.cs
using UnityEngine;
using TMPro;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("General Dialogue Settings")]
    [SerializeField] protected GameObject dialoguePanel;
    [SerializeField] protected string dialogueText = "Default text";
    [SerializeField] protected float fontSize = 24;
    [SerializeField] protected float displayDuration = 5f;

    [Header("Visual Cue")]
    [SerializeField] protected GameObject interactionPrompt;

    protected TMP_Text textComponent;
    protected Coroutine typingCoroutine;
    protected bool isTyping = false;

    protected virtual void Start()
    {
        InitializeDialoguePanel();
        ShowInteractionPrompt(false);
    }

    protected void InitializeDialoguePanel()
    {
        if (dialoguePanel != null)
        {
            textComponent = dialoguePanel.GetComponentInChildren<TMP_Text>();
            if (textComponent != null) textComponent.fontSize = fontSize;
            dialoguePanel.SetActive(false);
        }
    }

    public void ShowInteractionPrompt(bool show)
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(show);
    }

    protected void HideDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public abstract void TriggerInteraction();
    public abstract void SkipInteraction();

    // Optional runtime adjustments
    public void SetFontSize(int newSize)
    {
        fontSize = newSize;
        if (textComponent != null) textComponent.fontSize = fontSize;
    }

    public void SetDisplayDuration(float duration) => displayDuration = duration;
}