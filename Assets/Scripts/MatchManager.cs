using UnityEngine;
using TMPro;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject cardStack;

    [Header("Popup UI")]
    [SerializeField] GameObject matchPopupMiso;  // 🍜 Miso popup
    [SerializeField] GameObject matchPopupCina;  // 🐱 Cina popup
    [SerializeField] float popupAnimDuration = 0.3f;

    [Header("Dictionary for Matchable meems")]
    [SerializeField] string[] matchNames;

    [Header("Audio")]
    [SerializeField] string misoPopupSFX = "MisoMatchSFX";
    [SerializeField] string cinaPopupSFX = "CinaMatchSFX";

    public static bool isMisoMatched = false;
    public static bool isCinaMatched = false;

    RectTransform activePopupRect;
    Vector3 hiddenScale = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 shownScale = Vector3.one;

    void Awake()
    {
        isMisoMatched = false;
        isCinaMatched = false;

        if (matchPopupMiso != null) matchPopupMiso.SetActive(false);
        if (matchPopupCina != null) matchPopupCina.SetActive(false);
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
        // 🍜 Miso match
        if (cardName == "PREF_Card (26) match")
        {
            isMisoMatched = true;
            Debug.Log("[MatchManager] Miso matched!");
            ShowCharacterPopup(matchPopupMiso, misoPopupSFX);
        }
        // 🐱 Cina match
        else if (cardName == "PREF_Card (4) match")
        {
            isCinaMatched = true;
            Debug.Log("[MatchManager] Cina matched!");
            ShowCharacterPopup(matchPopupCina, cinaPopupSFX);
        }
    }

    void ShowCharacterPopup(GameObject popup, string sfxName)
    {
        // stop current bgm
        AudioManager.Instance.StopBGM();

        // play unique popup sfx
        if (!string.IsNullOrEmpty(sfxName))
            AudioManager.Instance.PlaySFX(sfxName);

        // enable popup
        popup.SetActive(true);
        cardStack.SetActive(false);
        hand.SetActive(false);

        activePopupRect = popup.GetComponent<RectTransform>();

        StopAllCoroutines();
        StartCoroutine(AnimatePopup());
    }

    IEnumerator AnimatePopup()
    {
        activePopupRect.localScale = hiddenScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / popupAnimDuration;
            activePopupRect.localScale = Vector3.Lerp(hiddenScale, shownScale, t);
            yield return null;
        }
    }
}
