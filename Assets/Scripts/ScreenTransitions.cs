using UnityEngine;
using System.Collections;

public class ScreenTransitions : MonoBehaviour
{

    [SerializeField] Transform chatScreen;
    [SerializeField] Vector3 hiddenPos = new Vector3(5.37f, 1.61f, -4.85f);
    [SerializeField] Vector3 visiblePos = new Vector3(0f, 1.61f, -4.85f);
    [SerializeField] float duration = 0.5f;

    public static bool enableCatChat = false;

    private Coroutine moveRoutine;

    void Start()
    {
        enableCatChat = false;
        chatScreen.position = hiddenPos;
    }

    public void ShowChatButton()
    {
        
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveTo(chatScreen, visiblePos, duration, true));
    }

    public void HideChatButton()
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveTo(chatScreen, hiddenPos, duration, false));
        
    }

    private IEnumerator MoveTo(Transform target, Vector3 targetPos, float time, bool showingChat)
    {
        enableCatChat = true;
        Vector3 start = target.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            target.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        target.position = targetPos;

    }
}
