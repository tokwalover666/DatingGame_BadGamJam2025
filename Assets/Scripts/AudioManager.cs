using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audios;

    private AudioSource[] audioSources;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        audioSource = GetComponent<AudioSource>();

    }

    public void PlaySwipeLeftSound()
    {
        PlayAudio("   ");
        Debug.Log("Play swipe left");

    }

    public void PlaySwipeRightSound()
    {

        PlayAudio("   ");
        Debug.Log("Play explosion");

    }


    private void PlayAudio(string audioName)
    {
        AudioClip clip = FindAudioByName(audioName);


        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();


    }

    private AudioClip FindAudioByName(string audioName)
    {
        foreach (AudioClip audio in audios)
        {
            if (audio.name == audioName)
            {
                return audio;
            }
        }
        return null;
    }

    private AudioSource GetAudioSourceByClip(AudioClip clip)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.clip == clip)
            {
                return source;
            }
        }
        return null;
    }

    public void SetPitchForAudioClip(string audioName, float pitch)
    {
        AudioClip clip = FindAudioByName(audioName);

        if (clip != null)
        {
            AudioSource source = GetAudioSourceByClip(clip);
            if (source != null)
            {
                source.pitch = pitch;
            }
            else
            {
                Debug.LogError("No AudioSource found playing the clip: " + audioName);
            }
        }
        else
        {
            Debug.LogError("Audio clip not found: " + audioName);
        }
    }
}