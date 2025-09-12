using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] RawImage fadeImage;
    [SerializeField] float fadeDuration = 1f;

    [Header("Pixelation")]
    [SerializeField] PostProcess postProcess;
    [SerializeField] int startScale = 10;
    [SerializeField] int endScale = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("[FadeManager] FadeImage is not assigned!");
            return;
        }

        fadeImage.color = Color.black;
        StartCoroutine(FadeOut());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(FadeIn());
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float normalized = t / fadeDuration;

            // Fade alpha
            c.a = Mathf.Lerp(0f, 1f, normalized);
            fadeImage.color = c;

            // Pixelation scale
            if (postProcess != null)
                postProcess.scale = Mathf.RoundToInt(Mathf.Lerp(endScale, startScale, normalized));

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float normalized = t / fadeDuration;

            // Fade alpha
            c.a = Mathf.Lerp(1f, 0f, normalized);
            fadeImage.color = c;

            // Pixelation scale
            if (postProcess != null)
                postProcess.scale = Mathf.RoundToInt(Mathf.Lerp(startScale, endScale, normalized));

            yield return null;
        }
    }
}
