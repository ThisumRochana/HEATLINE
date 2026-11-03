using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; // Array of all your background music tracks
    public static AudioManager instance;

    void Awake()
    {
        // Singleton Pattern: Makes sure only ONE Audio Manager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // This keeps music playing during scene loads

        // Set up an AudioSource component for every sound in our list
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Call this from any script: AudioManager.instance.Play("MusicName");
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // If it's already playing, don't restart it
        if (s.source.isPlaying) return;

        s.source.Play();
    }

    // Call this to stop a specific track
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Stop();
    }

    // Add this new method inside your AudioManager.cs class
    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null)
            {
                s.source.Stop();
            }
        }
    }
}