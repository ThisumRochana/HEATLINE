using UnityEngine;

public class PoliceSpawner : MonoBehaviour
{
    [Header("Police Prefabs")]
    public GameObject normalPolicePrefab;
    public GameObject heavyPolicePrefab;
    public GameObject helicopterPrefab;

    [Header("Spawn Settings")]
    public Transform player;
    public float normalSpawnInterval = 4f;

    [Header("Heavy Police Settings")]
    public float heavySpawnDelay = 120f;
    public float heavyRepeatMin = 60f;
    public float heavyRepeatMax = 90f;
    private bool heavyAlive = false;

    [Header("Helicopter Settings")]
    public float helicopterMinTime = 300f;
    public float helicopterMaxTime = 600f;

    [Header("Spawn Distance")]
    public float minSpawnDistance = 18f;
    public float maxSpawnDistance = 28f;

    [Header("UI")]
    public PoliceLevelUI policeLevelUI;

    [Header("Ground Placement")]
    public float spawnOffsetY = 0.5f; // 🔧 ADJUST THIS if car is stuck in ground or too high
    public LayerMask groundLayer;     // Optional: Select your "Ground" layer here to be safe

    private bool playerAlive = true;
    private int policeLevel = 1;

    void Start()
    {
        SetPoliceLevel(1);
        InvokeRepeating(nameof(SpawnNormalPolice), 2f, normalSpawnInterval);
        Invoke(nameof(SpawnHeavyPolice), heavySpawnDelay);
        Invoke(nameof(SpawnHelicopter), Random.Range(helicopterMinTime, helicopterMaxTime));
    }

    // ---------------- POLICE LEVEL ----------------
    void SetPoliceLevel(int level)
    {
        policeLevel = level;
        if (policeLevelUI != null) policeLevelUI.SetPoliceLevel(level);

        switch (policeLevel)
        {
            case 1: UpdateNormalSpawnInterval(4f); break;
            case 2: UpdateNormalSpawnInterval(3f); break;
            case 3: UpdateNormalSpawnInterval(2f); break;
        }
    }

    void UpdateNormalSpawnInterval(float newInterval)
    {
        normalSpawnInterval = newInterval;
        CancelInvoke(nameof(SpawnNormalPolice));
        if (playerAlive && player != null)
            InvokeRepeating(nameof(SpawnNormalPolice), 0.5f, normalSpawnInterval);
    }

    // ---------------- NORMAL POLICE ----------------
    void SpawnNormalPolice()
    {
        if (!playerAlive || player == null) return;
        SpawnPolice(normalPolicePrefab);
    }

    // ---------------- HEAVY POLICE ----------------
    void SpawnHeavyPolice()
    {
        if (!playerAlive || player == null || heavyAlive) return;

        SetPoliceLevel(2);
        GameObject heavy = SpawnPolice(heavyPolicePrefab);
        heavyAlive = true;

        PoliceCarHeavy heavyScript = heavy.GetComponent<PoliceCarHeavy>();
        if (heavyScript != null)
            heavyScript.OnDestroyCallback = OnHeavyPoliceDestroyed;
    }

    public void OnHeavyPoliceDestroyed()
    {
        heavyAlive = false;
        Invoke(nameof(SpawnHeavyPolice), Random.Range(heavyRepeatMin, heavyRepeatMax));
    }

    // ---------------- HELICOPTER ----------------
    void SpawnHelicopter()
    {
        if (!playerAlive || player == null) return;

        SetPoliceLevel(3);
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        float distance = maxSpawnDistance + 10f;
        Vector3 spawnPos = player.position + dir.normalized * distance;

        // Helicopter spawns high up, so we hardcode a height
        spawnPos.y = 15f;

        Instantiate(helicopterPrefab, spawnPos, Quaternion.identity);
        Invoke(nameof(SpawnHelicopter), Random.Range(helicopterMinTime, helicopterMaxTime));
    }

    // ---------------- COMMON SPAWN ----------------
    GameObject SpawnPolice(GameObject prefab)
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // 1. Calculate the X and Z position around the player
        Vector3 targetPos = player.position + dir.normalized * distance;

        // 2. Find the ground height using Raycast
        // Start from high up (e.g., Y=50) and shoot down
        Vector3 rayStart = new Vector3(targetPos.x, 50f, targetPos.z);
        Vector3 finalSpawnPos = targetPos;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            // Found the ground! Place car at hit point + offset
            finalSpawnPos = hit.point;
            finalSpawnPos.y += spawnOffsetY;
        }
        else
        {
            // Fallback: If no ground found, default to Y = 0 + offset
            finalSpawnPos.y = 0f + spawnOffsetY;
        }

        GameObject police = Instantiate(prefab, finalSpawnPos, Quaternion.identity);
        return police;
    }

    public void OnPlayerDeath()
    {
        playerAlive = false;
        CancelInvoke();
    }

    void Update()
    {
        if (!playerAlive) return;

        if (Time.timeSinceLevelLoad >= 600f && normalSpawnInterval > 1f)
        {
            UpdateNormalSpawnInterval(1f);
        }
    }
}