using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public SoundSettings[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    } 

    private void Start()
    {
        PlayMusic("THE GREAT SOUTHERN TRENDKILL");
    }

    public void PlayMusic(string name)
    {
        SoundSettings s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }

        else
        {
            musicSource.clip=s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        SoundSettings s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found.");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void PlayClick()
    {
        PlaySFX("VINE BOOM");
    }

    public void SetMusicVolume(float volume)
    
    {
        Debug.Log("Music volume set to: " + volume);
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    
}
