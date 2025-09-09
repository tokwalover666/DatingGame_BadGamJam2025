using UnityEngine;
using System.Collections.Generic;

public class SwipeController : MonoBehaviour
{
    public List<SwipeCard> cards = new List<SwipeCard>();
    int currentIndex = 0;

    SwipeCard CurrentCard => currentIndex < cards.Count ? cards[currentIndex] : null;

    void Start()
    {
        cards.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i).GetComponent<SwipeCard>();
            if (c != null) cards.Add(c);
        }
    }

    void Update()
    {
        if (CurrentCard == null) return;
        CurrentCard.HandleInput();
        if (CurrentCard.HasFlownOff) currentIndex++;
    }
}
