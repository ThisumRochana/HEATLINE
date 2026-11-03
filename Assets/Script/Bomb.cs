using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 50;
    public GameObject explosionFX;

    [Header("Homing")]
    public float speed = 20f;          // Movement speed
    public float rotateSpeed = 5f;     // How fast it turns toward target
    private Transform target;

    void Start()
    {
        target = GetClosestTarget();
    }

    void Update()
    {
        if (!target) return;

        // Homing movement
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = dir.y * 0.5f; // Adjust vertical aiming if needed

        // Smooth rotation
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.forward * speed * Time.deltaTime;
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

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Explosion FX
        if (explosionFX)
            Instantiate(explosionFX, transform.position, Quaternion.identity);

        // Damage nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // Damage Player / Clones
            if (col.CompareTag("Player"))
            {
                CarControl player = col.GetComponent<CarControl>();
                if (player != null)
                {
                    player.CrashPlayer();
                }
            }

            // Destroy Police
            if (col.CompareTag("Police"))
            {
                Destroy(col.gameObject);
            }
        }

        Destroy(gameObject);
    }
}
