using UnityEngine;

public class cameraAngel : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 6f;

    private Vector3 offset;       // Camera default offset from player
    private Vector3 shakeOffset;  // Offset added by shake

    void Awake()
    {
        if (!target)
            target = GameObject.FindWithTag("Player").transform;

        offset = transform.position - target.position;
        shakeOffset = Vector3.zero;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Apply follow + shake
        Vector3 desiredPos = target.position + offset + shakeOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        // Keep camera always looking at target
        transform.LookAt(target.position + Vector3.up * 1.5f); // Adjust Y for angled view
    }

    // Called by CameraShake
    public void ApplyShake(Vector3 shake)
    {
        shakeOffset = shake;
    }

    public void ResetShake()
    {
        shakeOffset = Vector3.zero;
    }

    // --------------------------
    // Add this method to fix FlightTransformPower
    // --------------------------
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // Recalculate offset so camera stays same relative distance
        if (target != null)
            offset = transform.position - target.position;
    }
}
