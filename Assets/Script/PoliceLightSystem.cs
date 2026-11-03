using UnityEngine;
using System.Collections;

public class PoliceLightSystem : MonoBehaviour
{
    public Light redLight;
    public Light blueLight;
    public float flashSpeed = 0.25f;
    public float maxIntensity = 7f;

    void Start()
    {
        StartCoroutine(FlashLights());
    }

    IEnumerator FlashLights()
    {
        while (true)
        {
            redLight.intensity = maxIntensity;
            blueLight.intensity = 0;
            yield return new WaitForSeconds(flashSpeed);

            redLight.intensity = 0;
            blueLight.intensity = maxIntensity;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
