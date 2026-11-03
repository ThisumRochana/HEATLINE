using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.35f;
    public float strength = 1.5f;

    private cameraAngel camFollow;

    void Awake()
    {
        camFollow = GetComponent<cameraAngel>();
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Only XZ plane for top-down / angled view
            Vector3 shake = new Vector3(
                Random.Range(-3f, 3f),
                0f,
                Random.Range(-3f, 3f)
            ) * strength;

            camFollow.ApplyShake(shake);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        camFollow.ResetShake();
    }
    public void ShakeOnce()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

}
