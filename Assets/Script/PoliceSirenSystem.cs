using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoliceSirenSystem : MonoBehaviour
{
    private AudioSource siren;
    private bool isStopped = false;

    void Start()
    {
        siren = GetComponent<AudioSource>();
        siren.loop = true;
        siren.Play();
    }

    // This is called by GameManager when player crashes
    public void StopSiren()
    {
        if (isStopped) return;

        isStopped = true;

        if (siren != null && siren.isPlaying)
            siren.Stop();
    }

    void OnDestroy()
    {
        if (siren != null)
            siren.Stop();
    }
}
