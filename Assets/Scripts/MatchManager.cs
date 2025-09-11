using UnityEngine;
using TMPro;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    [Header("Popup UI")]
    [SerializeField] GameObject matchPopup;    
    [SerializeField] float popupAnimDuration = 0.3f;

    [Header("Dictionary for Matchable meems")]
    [SerializeField] string[] matchNames;

    RectTransform popupRect;
    Vector3 hiddenScale = new Vector3(0.01071011f, 0.01071011f, 0.01071011f);
    Vector3 shownScale = new Vector3(1f, 1f, 1f);

    void Awake()
    {
        popupRect = matchPopup.GetComponent<RectTransform>();
    }

    public void CheckMatch(string cardName)
    {
        foreach (var name in matchNames)
        {
            if (cardName == name)
            {
                ShowPopup(cardName);
                return;
            }
        }
    }

    void ShowPopup(string cardName)
    {

        StopAllCoroutines();
        StartCoroutine(AnimatePopup());
    }

    IEnumerator AnimatePopup()
    {
        popupRect.localScale = hiddenScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / popupAnimDuration;
            popupRect.localScale = Vector3.Lerp(hiddenScale, shownScale, t);
            yield return null;
        }
    }
}
