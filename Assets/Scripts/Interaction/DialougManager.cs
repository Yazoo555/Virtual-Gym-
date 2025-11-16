using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject skipPrompt;

    [Header("Default Settings")]
    [SerializeField] private float defaultTypingSpeed = 0.05f;
    [SerializeField] private int defaultFontSize = 24;
    [SerializeField] private float defaultDisplayDuration = 5f;

    private Coroutine typingCoroutine;
    private Coroutine hideCoroutine;
    public InteractableNPC CurrentSpeaker { get; private set; }
    private string currentFullText;
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeUI();
    }

    private void InitializeUI()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            dialogueText.fontSize = defaultFontSize;
        }
        if (skipPrompt != null) skipPrompt.SetActive(false);
    }

    public void StartDialogue(string text, InteractableNPC speaker,
                            float typingSpeed = -1, int fontSize = -1,
                            float displayDuration = -1)
    {
        // Clear any existing dialogue
        HideDialogue(); // Changed from StopActiveDialogue() to HideDialogue()

        CurrentSpeaker = speaker;
        currentFullText = text;

        // Apply settings or use defaults
        float speed = typingSpeed > 0 ? typingSpeed : defaultTypingSpeed;
        int size = fontSize > 0 ? fontSize : defaultFontSize;
        float duration = displayDuration > 0 ? displayDuration : defaultDisplayDuration;

        // Configure UI
        dialogueText.fontSize = size;
        dialoguePanel.SetActive(true);
        if (skipPrompt != null) skipPrompt.SetActive(true);

        // Start typing effect
        typingCoroutine = StartCoroutine(TypeText(text, speed, duration));
    }

    private IEnumerator TypeText(string text, float speed, float duration)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
        if (skipPrompt != null) skipPrompt.SetActive(false);

        // Start auto-hide countdown
        hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDialogue();
    }

    public void SkipTyping()
    {
        if (!isTyping || CurrentSpeaker == null) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        dialogueText.text = currentFullText;
        isTyping = false;
        if (skipPrompt != null) skipPrompt.SetActive(false);

        // Restart hide timer
        hideCoroutine = StartCoroutine(HideAfterDelay(defaultDisplayDuration));
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        CurrentSpeaker = null;
        currentFullText = "";
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }

    public bool IsDialogueActive() => dialoguePanel != null && dialoguePanel.activeSelf;
}