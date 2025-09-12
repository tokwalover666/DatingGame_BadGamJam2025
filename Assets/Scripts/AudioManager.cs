using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] audios;

    private AudioSource sfxSource; // for sound effects
    private AudioSource bgmSource; // for background music

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Add two separate AudioSources
        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true; // bgm should loop
        sfxSource.loop = false; // sfx should not loop
    }

    // 🔊 SFX methods
    public void PlaySwipeLeftSound() => PlaySFX("LeftSwipe");
    public void PlaySwipeRightSound() => PlaySFX("RightSwipe");
    public void ChatNotif() => PlaySFX("Chat_Notif");
    public void ClickSound() => PlaySFX("Click");

    private void PlaySFX(string audioName)
    {
        AudioClip clip = FindAudioByName(audioName);
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // 🎵 BGM methods
    public void PlayBGM(string audioName)
    {
        AudioClip clip = FindAudioByName(audioName);
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // Helpers
    private AudioClip FindAudioByName(string audioName)
    {
        foreach (AudioClip audio in audios)
        {
            if (audio.name == audioName)
                return audio;
        }
        Debug.LogWarning("Audio not found: " + audioName);
        return null;
    }
}
