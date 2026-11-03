using UnityEngine;
using System.Collections;

public class SpeedPower : PowerUpBase
{
    public float speedMultiplier = 2f;

    protected override void ApplyPower(GameObject player)
    {
        CarControl controller = player.GetComponent<CarControl>();
        if (controller != null)
        {
            controller.StartCoroutine(SpeedBoost(controller));
        }
    }

    protected override void RemovePower(GameObject player)
    {
        CarControl controller = player.GetComponent<CarControl>();
        if (controller != null)
        {
            controller.ResetSpeed();
        }
    }

    IEnumerator SpeedBoost(CarControl controller)
    {
        Camera cam = Camera.main;
        float originalFOV = cam.fieldOfView;

        TrailRenderer trail = controller.GetComponent<TrailRenderer>();
        if (trail != null) trail.emitting = true;

        controller.ModifySpeed(speedMultiplier);
        cam.fieldOfView = originalFOV + 15f;

        yield return new WaitForSeconds(powerDuration);

        controller.ResetSpeed();
        cam.fieldOfView = originalFOV;
        if (trail != null) trail.emitting = false;
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate(other.gameObject); // From PowerUpBase
            Destroy(gameObject);        // Remove gift box
        }
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Transform cam = Camera.main.transform;
        Vector3 originalPos = cam.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = originalPos;
    }


}