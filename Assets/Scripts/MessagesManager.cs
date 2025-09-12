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
    [SerializeField] GameObject capuccinaMessages;
    [SerializeField] GameObject misoMessages;

    [Header("Final Step")]
    [SerializeField] Button dateButtonCina;
    [SerializeField] Button dateButtonMiso;

    private int currentCinaIndex = 0;
    private int currentMisoIndex = 0;
    private bool enableNextChatCapucinna = false;
    private bool enableNextChatMiso = false;

    void Start()
    {
        misoMessages.SetActive(false);
        foreach (var chat in capuccinaChats)
            chat.SetActive(false);

        foreach (var chat in misoChats)
            chat.SetActive(false);
         
        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);
         
        if (dateButtonCina != null) dateButtonCina.onClick.AddListener(ShowFinalImage);
         
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

    }

    void Update()
    {
        if (enableNextChatCapucinna && Input.GetMouseButtonDown(0))
            NextChatCapuccina();

        if (MatchManager.isMisoMatched == true)
        {
            capuccinaMessages.SetActive(false);
            misoMessages.SetActive(true);
        }

        if (MatchManager.isMisoMatched == true && ScreenTransitions.enableCatChat == true && Input.GetMouseButtonDown(0))
        {

            enableNextChatMiso = true;
            Debug.Log("miso");

            NextChatMiso();
        }
            
    }

    void ShowChatCapucinna()
    {
        if (currentCinaIndex < capuccinaChats.Count)
        {
            capuccinaChats[currentCinaIndex].SetActive(true);
            currentCinaIndex++;

            AudioManager.Instance.ChatNotif(); // 🔊 notif sound

            if (currentCinaIndex >= capuccinaChats.Count && dateButtonCina != null)
                dateButtonCina.gameObject.SetActive(true);
        }
    }

    void ShowChatMiso()
    {
        // Prevent running if index is already out of range
        if (currentMisoIndex >= misoChats.Count)
        {
            Debug.Log("[MessagesManager] No more Miso chats to show.");
            return;
        }

        // Show next chat
        misoChats[currentMisoIndex].SetActive(true);

        // 🔊 Play SFX here only when we actually activated one
        AudioManager.Instance.ChatNotif();

        currentMisoIndex++;

        // If we reached the end, show the date button
        if (currentMisoIndex >= misoChats.Count && dateButtonMiso != null)
            dateButtonMiso.gameObject.SetActive(true);
    }


    public void NextChatCapuccina()
    {
        if (enableNextChatCapucinna && currentCinaIndex < capuccinaChats.Count)
            ShowChatCapucinna();
    }

    public void NextChatMiso()
    {
        if (enableNextChatMiso && currentMisoIndex < misoChats.Count)
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
