using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    public CameraShake cameraShake;
    public AudioSource crashAudio;

    private bool isCrashed = false;

    void Start()
    {
        if (!cameraShake)
            cameraShake = Camera.main.GetComponent<CameraShake>();

        if (!crashAudio)
            crashAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCrashed) return;

        if (collision.gameObject.CompareTag("Police"))
        {
            isCrashed = true;

            Debug.Log("PLAYER CRASHED - GAME OVER");

            // 🔊 Sound
            if (crashAudio)
                crashAudio.Play();

            // 📷 Camera shake
            if (cameraShake)
                cameraShake.Shake();
        }
    }
}
