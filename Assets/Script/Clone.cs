using UnityEngine;

public class Clone : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float lifeTime = 6f;

    private Vector3 moveDir;

    void Start()
    {
        // Random direction on ground
        moveDir = Random.insideUnitSphere;
        moveDir.y = 0;
        moveDir.Normalize();

        // Rotate clone to face move direction
        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move forward continuously
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
