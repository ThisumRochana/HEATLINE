using UnityEngine;

public class CrashVFXSpawner : MonoBehaviour
{
    public GameObject[] crashVFXPrefabs;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Police") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            SpawnRandomVFX(collision.contacts[0].point);
        }
    }

    void SpawnRandomVFX(Vector3 pos)
    {
        if (crashVFXPrefabs == null || crashVFXPrefabs.Length == 0)
            return;

        int index = Random.Range(0, crashVFXPrefabs.Length);
        GameObject vfx = Instantiate(crashVFXPrefabs[index], pos, Quaternion.identity);

        // Optional: Rotate effect randomly
        vfx.transform.rotation = Random.rotation;

        // Auto destroy after effect duration
        Destroy(vfx, 2f);
    }
}
