using UnityEngine;
using System.Collections.Generic;

public class SwipeController : MonoBehaviour
{
    public List<SwipeCard> cards = new List<SwipeCard>();
    int currentIndex = 0;

    SwipeCard CurrentCard => currentIndex < cards.Count ? cards[currentIndex] : null;

    [SerializeField] float cardSpacing = 0.05f;
    [SerializeField] float reorderSpeed = 5f;
     
    [SerializeField] GameObject swipedLeftAnim;
    [SerializeField] GameObject swipedRightAnim;
    [SerializeField] GameObject idleAnim;

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

        SetIdleAnim();
    }

    void Update()
    {
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
        }
        else if (!Input.GetMouseButton(0) && !CurrentCard.HasFlownOff)
        { 
            SetIdleAnim();
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
        idleAnim.SetActive(true);
        swipedLeftAnim.SetActive(false);
        swipedRightAnim.SetActive(false);
    }

    void SetLeftAnim()
    {
        idleAnim.SetActive(false);
        swipedLeftAnim.SetActive(true);
        swipedRightAnim.SetActive(false);
    }

    void SetRightAnim()
    {
        idleAnim.SetActive(false);
        swipedLeftAnim.SetActive(false);
        swipedRightAnim.SetActive(true);
    }
}
