using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PoliceCarHeavy : MonoBehaviour
{
    public System.Action OnDestroyCallback;

    public float moveSpeed = 18f;
    public float turnSpeed = 6f;

    public float health = 100f;
    public float crashDamage = 35f;
    public float ramForce = 450f;

    public float separationRadius = 4f;
    public float separationForce = 15f;

    private Rigidbody rb;
    private Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ✅ ENABLE GRAVITY
        rb.useGravity = true;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        // ❌ REMOVED: No longer forcing Y position manually

        target = GetClosestTarget();
        if (!target) return;

        ApplySeparation();
        ChaseTarget();
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
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, turnSpeed * Time.fixedDeltaTime));

        rb.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
    }

    void ApplySeparation()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, separationRadius);
        foreach (Collider col in nearby)
        {
            if (col.CompareTag("Police") && col.gameObject != gameObject)
            {
                Vector3 push = transform.position - col.transform.position;
                push.y = 0;
                rb.AddForce(push.normalized * separationForce, ForceMode.Force);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Police") ||
            collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(crashDamage);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDestroyCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}