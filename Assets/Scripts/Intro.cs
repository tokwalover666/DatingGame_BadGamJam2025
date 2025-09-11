using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Intro : MonoBehaviour
{
    [Header("Display")]
    public Image displayImage;              
    public Sprite[] images;                 
    public TransitionType[] transitions;              

    [Header("Animation Settings")]
    public float spinSpeed = 720f;          
    public float fadeDuration = 1f;
    public float shakeAmount = 10f;
    public float shakeSpeed = 20f;
    public float zoomScale = 2f;
    public float zoomDuration = 1f;
    public float slideDistance = 1000f;
    public float slideDuration = 1f;

    private int currentIndex = 0;
    private RectTransform rectTransform;

    public enum TransitionType { None, Spin, Fade, Shake, Zoom, Slide }

    void Start()
    {
        rectTransform = displayImage.GetComponent<RectTransform>();
        if (images.Length > 0)
        {
            displayImage.sprite = images[0];
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            StartCoroutine(ShowNextImage());
        }
    }

    IEnumerator ShowNextImage()
    {
        if (currentIndex < images.Length)
        {
            // Play transition
            if (currentIndex < transitions.Length)
            {
                switch (transitions[currentIndex])
                {
                    case TransitionType.Spin:
                        yield return StartCoroutine(SpinAnimation());
                        break;
                    case TransitionType.Fade:
                        yield return StartCoroutine(FadeAnimation());
                        break;
                    case TransitionType.Shake:
                        yield return StartCoroutine(ShakeAnimation());
                        break;
                    case TransitionType.Zoom:
                        yield return StartCoroutine(ZoomAnimation());
                        break;
                    case TransitionType.Slide:
                        yield return StartCoroutine(SlideAnimation());
                        break;
                }
            }

            rectTransform.rotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            displayImage.color = Color.white;

            currentIndex++;
            if (currentIndex < images.Length)
            {
                displayImage.sprite = images[currentIndex];
            }
            else
            {
                SceneManager.LoadScene("2_Gameplay");
            }
        }
    }

    IEnumerator SpinAnimation()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            rectTransform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FadeAnimation()
    {
        float t = 0f;
        Color c = displayImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            displayImage.color = c;
            yield return null;
        }
    }

    IEnumerator ShakeAnimation()
    {
        Vector3 startPos = rectTransform.localPosition;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * shakeSpeed;
            rectTransform.localPosition = startPos + new Vector3(Mathf.Sin(t * 20f) * shakeAmount, 0, 0);
            yield return null;
        }
        rectTransform.localPosition = startPos;
    }

    IEnumerator ZoomAnimation()
    {
        float t = 0f;
        Vector3 startScale = rectTransform.localScale;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            rectTransform.localScale = Vector3.Lerp(startScale, startScale * zoomScale, t / zoomDuration);
            yield return null;
        }
    }

    IEnumerator SlideAnimation()
    {
        float t = 0f;
        Vector3 startPos = rectTransform.localPosition;
        Vector3 endPos = startPos + Vector3.right * slideDistance;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, t / slideDuration);
            yield return null;
        }
    }
}
