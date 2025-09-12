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

    private int currentChatIndex = 0;
    private bool enableNextChatCapucinna = false;
    private bool enableNextChatMiso = false;

    void Start()
    {
        foreach (var chat in capuccinaChats)
            chat.SetActive(false);

        foreach (var chat in misoChats)
            chat.SetActive(false);

        // Hide final UI elements
        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);

        // Add listeners
        if (dateButtonCina != null) dateButtonCina.onClick.AddListener(ShowFinalImage);

        // 🎵 Start background music for chats
        AudioManager.Instance.PlayBGM("ChatBGM");
    }

    public void ClickChoice1()
    {
        AudioManager.Instance.ClickSound(); // 🔊 SFX
        chatHeadTMP.text = "hello hot stuff";
        enableNextChatCapucinna = true;

        // Hide choice buttons immediately
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChatCapucinna();
    }

    public void ClickChoice2()
    {
        AudioManager.Instance.ClickSound(); // 🔊 SFX
        chatHeadTMP.text = "hey…";
        enableNextChatCapucinna = true;

        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        ShowChatCapucinna();
    }

    public void ClickMisoScreen()
    {
        AudioManager.Instance.ClickSound(); // 🔊 SFX
        enableNextChatMiso = true;
        Debug.Log("miso");

        ShowChatMiso();
    }

    void Update()
    {
        if (enableNextChatCapucinna && Input.GetMouseButtonDown(0))
            NextChatCapuccina();

        if (enableNextChatMiso && Input.GetMouseButtonDown(0))
            NextChatMiso();
    }

    void ShowChatCapucinna()
    {
        if (currentChatIndex < capuccinaChats.Count)
        {
            capuccinaChats[currentChatIndex].SetActive(true);
            currentChatIndex++;

            AudioManager.Instance.ChatNotif(); // 🔊 notif sound

            if (currentChatIndex >= capuccinaChats.Count && dateButtonCina != null)
                dateButtonCina.gameObject.SetActive(true);
        }
    }

    void ShowChatMiso()
    {
        if (currentChatIndex < misoChats.Count)
        {
            misoChats[currentChatIndex].SetActive(true);
            currentChatIndex++;

            AudioManager.Instance.ChatNotif(); // 🔊 notif sound

            if (currentChatIndex >= misoChats.Count && dateButtonMiso != null)
                dateButtonMiso.gameObject.SetActive(true);
        }
    }

    public void NextChatCapuccina()
    {
        if (enableNextChatCapucinna && currentChatIndex < capuccinaChats.Count)
            ShowChatCapucinna();
    }

    public void NextChatMiso()
    {
        if (enableNextChatMiso && currentChatIndex < misoChats.Count)
            ShowChatMiso();
    }

    void ShowFinalImage()
    {
        // Hide all chats
        foreach (var chat in capuccinaChats)
            chat.SetActive(false);

        // Hide final button
        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);

        // 🎵 Switch music to romance
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("Romance");  // <-- fixed

        if (chatHeadTMP != null) chatHeadTMP.text = "";

        SceneManager.LoadScene("3_EndingCutscene");
    }

}
