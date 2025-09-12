using UnityEngine;
using TMPro;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject cardStack;

    [Header("Popup UI")]
    [SerializeField] GameObject matchPopup;
    [SerializeField] float popupAnimDuration = 0.3f;

    [Header("Dictionary for Matchable meems")]
    [SerializeField] string[] matchNames;
     
    public static bool isMisoMatched = false; 

    RectTransform popupRect;
    Vector3 hiddenScale = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 shownScale = Vector3.one;

    void Awake()
    {
        isMisoMatched = false;
        popupRect = matchPopup.GetComponent<RectTransform>();
        matchPopup.SetActive(false);
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
        if (cardName == "PREF_Card (26) match")
        {
            isMisoMatched = true;
            Debug.Log("[MatchManager] Special match! isMisoMatched = true");
        }

        matchPopup.SetActive(true);
        cardStack.SetActive(false);
        hand.SetActive(false);

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
