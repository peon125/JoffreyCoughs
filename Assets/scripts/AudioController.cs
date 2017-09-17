using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController _instance;
    public AudioSource soundtrackSource;
    public AudioSource soundSource;

    void Awake()
    {
        if (AudioController._instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlaySoundtrack(AudioClip audioClip)
    {
        if (audioClip != soundtrackSource.clip)
        {
            soundtrackSource.Stop();
            soundtrackSource.clip = audioClip;
            soundtrackSource.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        soundSource.Stop();
        soundSource.clip = audioClip;
        soundSource.Play();
    }
}