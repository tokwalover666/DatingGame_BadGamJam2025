using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] List<GameObject> capuccinaChats = new List<GameObject>();
    [SerializeField] TextMeshProUGUI chatHeadTMP;

    private int currentChatIndex = 0;
    private bool enableNextChat = false;


    void Start()
    {

        foreach (var chat in capuccinaChats)
        {
            chat.SetActive(false);
        }
    }

    public void ClickChoice1()
    {
        chatHeadTMP.text = "hello hot stuff";
        enableNextChat = true;
        capuccinaChats[currentChatIndex].SetActive(true);
        currentChatIndex++;
    }

    public void ClickChoice2()
    {
        chatHeadTMP.text = "hey…";
        enableNextChat = true;
    }

    void Update()
    {
        if (enableNextChat && Input.GetMouseButtonDown(0))
        {
            NextChatCapuccina();
        }
    }

    public void NextChatCapuccina()
    {
        if (enableNextChat && currentChatIndex < capuccinaChats.Count)
        {
            capuccinaChats[currentChatIndex].SetActive(true);
            currentChatIndex++;
        }
    }
}
