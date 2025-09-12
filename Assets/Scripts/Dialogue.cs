using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image dialogueImage;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue Content")]
    [SerializeField] private Sprite[] dialogueImages;
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines;

    [Header("Settings")]
    [SerializeField] private float typeSpeed = 0.03f;

    [Header("Audio")]
    [SerializeField] private string specialSFX = "booo"; // 🎵 sound for line 15

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (dialogueImages.Length != dialogueLines.Length)
        {
            Debug.LogError("⚠️ DialogueManager: Images and texts must have the same length!");
            return;
        }

        ShowDialogueSet(currentIndex);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentIndex];
                isTyping = false;
            }
            else
            {
                NextDialogue();
            }
        }
    }

    private void ShowDialogueSet(int index)
    {
        if (index >= dialogueLines.Length) return;

        // swap sprite
        if (dialogueImage != null && dialogueImages[index] != null)
            dialogueImage.sprite = dialogueImages[index];

        // ✅ check for line 15 (index 14)
        if (index == 14 && !string.IsNullOrEmpty(specialSFX))
        {
            AudioManager.Instance.PlaySFX(specialSFX);
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(dialogueLines[index]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    private void NextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogueLines.Length)
        {
            ShowDialogueSet(currentIndex);
        }
        else
        {
            dialogueImage.gameObject.SetActive(false);
            dialogueText.gameObject.SetActive(false);
        }
    }
}
