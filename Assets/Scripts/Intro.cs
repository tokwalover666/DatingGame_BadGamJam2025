using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("Display")]
    public Image displayImage;  
    public Sprite[] images;          
    private int currentIndex = 0;

    void Start()
    {
        AudioManager.Instance.PlayBGM("balay");
        displayImage.sprite = images[currentIndex];
    }

    void Update()
    {
        if (displayImage == null || images == null || images.Length == 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            currentIndex++;

            if (currentIndex < images.Length)
            {
                displayImage.sprite = images[currentIndex];
            }
            else
            {
                if (FadeManager.Instance != null)
                {
                    FadeManager.Instance.FadeToScene("2_Gameplay");
                }
        
            }
        }
    }
}
