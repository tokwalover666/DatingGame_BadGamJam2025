using UnityEngine;
using System.Collections.Generic;

public class SwipeController : MonoBehaviour
{
    public List<SwipeCard> cards = new List<SwipeCard>();
    int currentIndex = 0;

    SwipeCard CurrentCard => currentIndex < cards.Count ? cards[currentIndex] : null;

    [SerializeField] float cardSpacing = 0.05f;
    [SerializeField] float reorderSpeed = 5f;

    [Header("Animations")]
    [SerializeField] GameObject swipedLeftAnim;
    [SerializeField] GameObject swipedRightAnim;
    [SerializeField] GameObject idleAnim;

    [Header("Fade Settings")]
    [SerializeField] float fadeSpeed = 5f;

    [Header("UI Panels")]
    [SerializeField] GameObject settingsPanel; // 👈 assign in Inspector

    SpriteRenderer idleRenderer;
    float idleTargetAlpha = 1f;
    float idleCurrentAlpha = 1f;

    void Start()
    {
        cards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i).GetComponent<SwipeCard>();
            if (c != null)
            {
                cards.Add(c);
                c.OnDestroyed += HandleCardDestroyed;

                Vector3 pos = c.transform.localPosition;
                pos.z += i * cardSpacing;
                c.transform.localPosition = pos;
            }
        }

        if (idleAnim != null)
        {
            idleRenderer = idleAnim.GetComponent<SpriteRenderer>();
            if (idleRenderer != null)
            {
                Color c = idleRenderer.color;
                c.a = 1f;
                idleRenderer.color = c;
            }
        }

        SetIdleAnim();
    }

    void Update()
    {
        // 👇 Block swipe input if settings panel is active
        if (settingsPanel != null && settingsPanel.activeSelf)
            return;

        if (CurrentCard == null) return;
        CurrentCard.HandleInput();

        float halfway = Screen.width * 0.25f;
        float delta = Input.mousePosition.x - (Screen.width * 0.5f);

        if (Input.GetMouseButton(0))
        {
            if (Mathf.Abs(delta) > halfway)
            {
                if (delta > 0) SetRightAnim();
                else SetLeftAnim();
            }
            else
            {
                SetIdleAnim();
            }

            FadeIdle(0f);
        }
        else if (!Input.GetMouseButton(0) && !CurrentCard.HasFlownOff)
        {
            SetIdleAnim();
            FadeIdle(1f);
        }

        if (idleRenderer != null)
        {
            idleCurrentAlpha = Mathf.Lerp(idleCurrentAlpha, idleTargetAlpha, Time.deltaTime * fadeSpeed);
            Color c = idleRenderer.color;
            c.a = idleCurrentAlpha;
            idleRenderer.color = c;
        }

        ReorderCardsSmooth();
    }

    void HandleCardDestroyed(SwipeCard card)
    {
        currentIndex++;
        ReorderCardsSmooth();
    }

    void ReorderCardsSmooth()
    {
        for (int i = currentIndex; i < cards.Count; i++)
        {
            if (cards[i] != null)
            {
                Vector3 pos = cards[i].transform.localPosition;
                float targetZ = (i - currentIndex) * cardSpacing;
                pos.z = Mathf.Lerp(pos.z, targetZ, Time.deltaTime * reorderSpeed);
                cards[i].transform.localPosition = pos;
            }
        }
    }

    void SetIdleAnim()
    {
        if (idleAnim != null) idleAnim.SetActive(true);
        if (swipedLeftAnim != null) swipedLeftAnim.SetActive(false);
        if (swipedRightAnim != null) swipedRightAnim.SetActive(false);
    }

    void SetLeftAnim()
    {
        if (swipedLeftAnim != null) swipedLeftAnim.SetActive(true);
        if (swipedRightAnim != null) swipedRightAnim.SetActive(false);
    }

    void SetRightAnim()
    {
        if (swipedLeftAnim != null) swipedLeftAnim.SetActive(false);
        if (swipedRightAnim != null) swipedRightAnim.SetActive(true);
    }

    void FadeIdle(float targetAlpha)
    {
        idleTargetAlpha = targetAlpha;
    }
}
