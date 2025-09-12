using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] List<GameObject> capuccinaChats = new List<GameObject>();
    [SerializeField] List<GameObject> misoChats = new List<GameObject>();
    [SerializeField] TextMeshProUGUI chatHeadTMP;

    [Header("Choices")]
    [SerializeField] Button choiceButton1;
    [SerializeField] Button choiceButton2;


    [Header("Final Step")]
    [SerializeField] Button dateButtonCina;
    [SerializeField] Button dateButtonMiso;
    [SerializeField] GameObject finalImage;

    private int currentChatIndex = 0;
    private bool enableNextChatCapucinna = false;
    private bool enableNextChatMiso = false;

    void Start()
    {

        foreach (var chat in capuccinaChats)
        {
            chat.SetActive(false);
        }

        foreach (var chat in misoChats)
        {
            chat.SetActive(false);
        }

        // Hide final UI elements
        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);
        if (finalImage != null) finalImage.SetActive(false);

        // Add listeners
        if (dateButtonCina != null) dateButtonCina.onClick.AddListener(ShowFinalImage);
    }

    public void ClickChoice1()
    {
        AudioManager.Instance.ClickSound();
        chatHeadTMP.text = "hello hot stuff";
        enableNextChatCapucinna = true;

        // Hide choice buttons immediately
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChatCapucinna();
    }

    public void ClickChoice2()
    {
        AudioManager.Instance.ClickSound();
        chatHeadTMP.text = "hey…";
        enableNextChatCapucinna = true;

        // Hide choice buttons immediately
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChatCapucinna();
    }

    public void ClickMisoScreen()
    {
        AudioManager.Instance.ClickSound();
        enableNextChatMiso = true;
        Debug.Log("miso");

        ShowChatMiso();

    }

    void Update()
    {
        if (enableNextChatCapucinna && Input.GetMouseButtonDown(0))
        {
            NextChatCapuccina();
        }

        if (enableNextChatMiso && Input.GetMouseButtonDown(0))
        {
            NextChatMiso();
        }
    }

    void ShowChatCapucinna()
    {
        if (currentChatIndex < capuccinaChats.Count)
        {
            capuccinaChats[currentChatIndex].SetActive(true);
            currentChatIndex++;
            AudioManager.Instance.ChatNotif();

            if (currentChatIndex >= capuccinaChats.Count && dateButtonCina != null)
            {
                dateButtonCina.gameObject.SetActive(true);
            }
        }
    }

    void ShowChatMiso()
    {
        if (currentChatIndex < misoChats.Count)
        {
            misoChats[currentChatIndex].SetActive(true);
            currentChatIndex++;

            if (currentChatIndex >= misoChats.Count && dateButtonMiso != null)
            {
                dateButtonMiso.gameObject.SetActive(true);
            }
        }
    }

    public void NextChatCapuccina()
    {
        if (enableNextChatCapucinna && currentChatIndex < capuccinaChats.Count)
        {
            ShowChatCapucinna();
        }
    }

    public void NextChatMiso()
    {
        if (enableNextChatMiso && currentChatIndex < misoChats.Count)
        {
            ShowChatMiso();
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
        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);

        // Show final image
        if (finalImage != null) finalImage.SetActive(true);

        // Clear chat head text
        if (chatHeadTMP != null) chatHeadTMP.text = "";
    }
}
