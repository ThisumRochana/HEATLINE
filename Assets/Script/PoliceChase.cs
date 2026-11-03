using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PoliceCar : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 18f;
    public float turnSpeed = 6f;
    public float maxSpeed = 22f;

    [Header("Crash FX")]
    public GameObject crashFX;

    [Header("Separation")]
    public float separationRadius = 4f;
    public float separationForce = 12f;

    private Rigidbody rb;
    private Transform target; // nearest player/clone
    private bool isDestroyed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ✅ ENABLE GRAVITY so the car sits on the floor
        rb.useGravity = true;

        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Keep rotation frozen so it doesn't flip over easily
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // ❌ REMOVED: No longer forcing Y position manually

        target = GetClosestTarget();
        if (!target || isDestroyed) return;

        ApplySeparation();
        ChaseTarget();
        LimitSpeed();
    }

    Transform GetClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t.transform;
            }
        }
        return closest;
    }

    void ChaseTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Keep looking horizontally, even if on a hill

        if (direction.magnitude < 1f) return;

        direction.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));

        rb.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    void ApplySeparation()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, separationRadius);
        foreach (Collider col in nearby)
        {
            if (col.gameObject != gameObject && col.CompareTag("Police"))
            {
                Vector3 pushDir = transform.position - col.transform.position;
                pushDir.y = 0f;
                rb.AddForce(pushDir.normalized * separationForce, ForceMode.Force);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Police") ||
            collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            DestroyPolice();
        }
    }

    void DestroyPolice()
    {
        isDestroyed = true;

        if (crashFX != null)
            Instantiate(crashFX, transform.position, Quaternion.identity);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(10);

        Destroy(gameObject);
    }
}