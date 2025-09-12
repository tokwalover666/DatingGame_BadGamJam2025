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
    private bool hasStoppedBGM = false;

    void Start()
    {
        AudioManager.Instance.PlayBGM("SOUND 1");
        misoMessages.SetActive(false);
        foreach (var chat in capuccinaChats)
            chat.SetActive(false);

        foreach (var chat in misoChats)
            chat.SetActive(false);

        if (dateButtonCina != null)
        {
            dateButtonCina.gameObject.SetActive(false);
            dateButtonCina.onClick.AddListener(ShowFinalImageCina);
        }

        if (dateButtonMiso != null)
        {
            dateButtonMiso.gameObject.SetActive(false);
            dateButtonMiso.onClick.AddListener(ShowFinalImageMiso);
        }

        AudioManager.Instance.PlayBGM("ChatBGM");
    }

    public void ClickChoice1()
    {
        AudioManager.Instance.ClickSound(); // 🔊 SFX
        chatHeadTMP.text = "hello hot stuff";
        enableNextChatCapucinna = true;

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
        // placeholder for direct click logic if needed
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
            // ✅ stop popup SFX the first time messages appear
            AudioManager.Instance.StopSFX();

            if (!hasStoppedBGM)
            {
                AudioManager.Instance.StopBGM();
                hasStoppedBGM = true;
            }

            capuccinaChats[currentCinaIndex].SetActive(true);
            currentCinaIndex++;

            AudioManager.Instance.ChatNotif();

            if (currentCinaIndex >= capuccinaChats.Count && dateButtonCina != null)
                dateButtonCina.gameObject.SetActive(true);
        }
    }

    void ShowChatMiso()
    {
        if (currentMisoIndex >= misoChats.Count)
        {
            Debug.Log("[MessagesManager] No more Miso chats to show.");
            return;
        }

        // ✅ stop popup SFX the first time messages appear
        AudioManager.Instance.StopSFX();

        if (!hasStoppedBGM)
        {
            AudioManager.Instance.StopBGM();
            hasStoppedBGM = true;
        }

        misoChats[currentMisoIndex].SetActive(true);
        AudioManager.Instance.ChatNotif();
        currentMisoIndex++;

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

    void ShowFinalImageCina()
    {
        foreach (var chat in capuccinaChats)
            chat.SetActive(false);

        if (dateButtonCina != null) dateButtonCina.gameObject.SetActive(false);

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("Romance");

        if (chatHeadTMP != null) chatHeadTMP.text = "";

        SceneManager.LoadScene("3_CinaEnding");
    }

    void ShowFinalImageMiso()
    {
        foreach (var chat in misoChats)
            chat.SetActive(false);

        if (dateButtonMiso != null) dateButtonMiso.gameObject.SetActive(false);

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("Romance");

        if (chatHeadTMP != null) chatHeadTMP.text = "";

        SceneManager.LoadScene("4_MisoEnding");
    }
}
