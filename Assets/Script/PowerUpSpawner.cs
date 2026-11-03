using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;

    public Transform player;

    public float spawnInterval = 5f;
    public float spawnDistance = 40f;
    public float laneWidth = 4f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), 2f, spawnInterval);
    }

    void SpawnPowerUp()
    {
        if (player == null || powerUpPrefabs.Length == 0) return;

        Vector3 spawnPos = player.position + player.forward * spawnDistance;

        float offsetX = Random.Range(-laneWidth, laneWidth);
        spawnPos += player.right * offsetX;

        spawnPos.y = 0.5f; // adjust to road height

        int index = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[index], spawnPos, Quaternion.identity);
    }
}
