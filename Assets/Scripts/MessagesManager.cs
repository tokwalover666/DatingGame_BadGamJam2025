using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] List<GameObject> capuccinaChats = new List<GameObject>();
    [SerializeField] TextMeshProUGUI chatHeadTMP;

    [Header("Choices")]
    [SerializeField] Button choiceButton1;
    [SerializeField] Button choiceButton2;

    [Header("Final Step")]
    [SerializeField] Button finalButton;
    [SerializeField] GameObject finalImage;

    private int currentChatIndex = 0;
    private bool enableNextChat = false;

    void Start()
    {
        // Hide all chats at start
        foreach (var chat in capuccinaChats)
        {
            chat.SetActive(false);
        }

        // Hide final UI elements
        if (finalButton != null) finalButton.gameObject.SetActive(false);
        if (finalImage != null) finalImage.SetActive(false);

        // Add listeners
        if (finalButton != null) finalButton.onClick.AddListener(ShowFinalImage);
    }

    public void ClickChoice1()
    {
        chatHeadTMP.text = "hello hot stuff";
        enableNextChat = true;

        // Hide choice buttons immediately
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChat();
    }

    public void ClickChoice2()
    {
        chatHeadTMP.text = "hey…";
        enableNextChat = true;

        // Hide choice buttons immediately
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChat();
    }

    void Update()
    {
        if (enableNextChat && Input.GetMouseButtonDown(0))
        {
            NextChatCapuccina();
        }
    }

    void ShowChat()
    {
        if (currentChatIndex < capuccinaChats.Count)
        {
            capuccinaChats[currentChatIndex].SetActive(true);
            currentChatIndex++;

            if (currentChatIndex >= capuccinaChats.Count && finalButton != null)
            {
                finalButton.gameObject.SetActive(true);
            }
        }
    }

    public void NextChatCapuccina()
    {
        if (enableNextChat && currentChatIndex < capuccinaChats.Count)
        {
            ShowChat();
        }
    }

    void ShowFinalImage()
    {
        // Hide all chats
        foreach (var chat in capuccinaChats)
        {
            chat.SetActive(false);
        }

        // Hide final button
        if (finalButton != null) finalButton.gameObject.SetActive(false);

        // Show final image
        if (finalImage != null) finalImage.SetActive(true);

        // Clear chat head text
        if (chatHeadTMP != null) chatHeadTMP.text = "";
    }
}
