using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip win;

    [Header("Music")]
    [SerializeField] private AudioClip soundtrack;

    private AudioSource sfxSource;
    private AudioSource musicSource;

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
        }

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
    }
    public void PlaySound(int index)
    {
        AudioClip clip = index switch
        {
            1 => jump,
            2 => walk,
            3 => hit,
            4 => lose,
            5 => win,
        };

        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic()
    {
        musicSource.clip = soundtrack;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
