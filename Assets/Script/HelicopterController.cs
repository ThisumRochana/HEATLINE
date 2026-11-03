using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [Header("Parts")]
    public Transform mainRotor;
    public Transform tailRotor;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float hoverHeight = 20f;
    public float attackDuration = 10f;
    public float exitHeight = 50f;

    [Header("Bombing")]
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    public float bombInterval = 2f;

    private Transform player;
    private float attackTimer;
    private bool exiting = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        InvokeRepeating(nameof(DropBomb), 2f, bombInterval);
    }

    void Update()
    {
        RotateRotors();

        if (!exiting)
        {
            FlyTowardsPlayer();
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                exiting = true;
                CancelInvoke(nameof(DropBomb));
            }
        }
        else
        {
            FlyAway();
        }
    }

    void RotateRotors()
    {
        if (mainRotor) mainRotor.Rotate(Vector3.up * 1200f * Time.deltaTime);
        if (tailRotor) tailRotor.Rotate(Vector3.right * 1200f * Time.deltaTime);
    }

    void FlyTowardsPlayer()
    {
        if (!player) return;

        Vector3 targetPos = player.position + Vector3.up * hoverHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        LookAtPlayer();
    }

    void FlyAway()
    {
        Vector3 exitPos = transform.position + Vector3.up * exitHeight;
        transform.position = Vector3.MoveTowards(transform.position, exitPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, exitPos) < 1f)
            Destroy(gameObject);
    }

    void LookAtPlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 2f * Time.deltaTime);
    }

    void DropBomb()
    {
        if (!player || !bombPrefab || !bombSpawnPoint) return;

        Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation);
    }




}
