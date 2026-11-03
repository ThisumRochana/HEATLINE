using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    [Header("Music Settings")]
    [Tooltip("Type the EXACT name you gave the sound in the AudioManager list")]
    public string musicName;

    [Tooltip("Check this if you want to stop all other music when this scene starts")]
    public bool stopOtherMusic = true;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            if (stopOtherMusic)
            {
                // Stop everything else before playing the new track
                AudioManager.instance.StopAll();
            }

            AudioManager.instance.Play(musicName);
        }
    }
}