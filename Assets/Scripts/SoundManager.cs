using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> sounds;
    private Dictionary<string, AudioSource> SoundSources;

    public List<Sound> music;
    private Dictionary<string, AudioSource> MusicSources;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(this);

        SoundSources = new Dictionary<string, AudioSource>();
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.playOnAwake = sound.onAwake;

            SoundSources.Add(sound.name, source);
        }

        MusicSources = new Dictionary<string, AudioSource>();
        foreach (Sound sound in music)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.playOnAwake = sound.onAwake;

            MusicSources.Add(sound.name, source);
        }
    }

    public void SetVolume(string clipname, float volume)
    {
        MusicSources[clipname].volume = volume;
    }

    public void PlaySound(string soundName)
    {
        if (SoundSources.ContainsKey(soundName))
        {
            SoundSources[soundName].Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void StopSound(string soundName)
    {
        if (SoundSources.ContainsKey(soundName))
        {
            SoundSources[soundName].Stop();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void PlayMusic(string soundName)
    {
        if (MusicSources.ContainsKey(soundName))
        {
            MusicSources[soundName].Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void StopMusic(string soundName)
    {
        if (MusicSources.ContainsKey(soundName))
        {
            MusicSources[soundName].Stop();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool onAwake;
}

