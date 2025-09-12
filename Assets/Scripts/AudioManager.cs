using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] audios;

    private AudioSource sfxSource;
    private AudioSource bgmSource;

    private string currentBGMName = ""; // 👈 remember last BGM

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

        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        sfxSource.loop = false;

        bgmSource.volume = 1f;
        sfxSource.volume = 1f;
    }

    // 🔊 SFX
    public void PlaySwipeLeftSound() => PlaySFX("LeftSwipe");
    public void PlaySwipeRightSound() => PlaySFX("RightSwipe");
    public void ChatNotif() => PlaySFX("Chat_Notif");
    public void ClickSound() => PlaySFX("Click");

    public void PlaySFX(string audioName)
    {
        AudioClip clip = FindAudioByName(audioName);
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // 🎵 BGM
    public void PlayBGM(string audioName)
    {
        AudioClip clip = FindAudioByName(audioName);
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying) return; // already playing

        currentBGMName = audioName; // remember which track
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSFX()
    {
        if (sfxSource != null)
            sfxSource.Stop();
    }

    // 🎚 Volume control
    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }

    public void SetBGMVolume(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;
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
