using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] // This allows it to show up in the Inspector
public class Sound
{
    public string name;      // Example: "MainMenu" or "BattleLevel"
    public AudioClip clip;   // Your .mp3 or .wav file

    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop = true;

    [HideInInspector] // We create this via code, no need to see it in Inspector
    public AudioSource source;
}