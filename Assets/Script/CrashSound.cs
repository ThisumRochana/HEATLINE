using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CrashSound : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCrash()
    {
        audioSource.Play();
    }
}
