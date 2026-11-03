using UnityEngine;
using System.Collections;

public class ClonePower : MonoBehaviour // Changed to MonoBehaviour for easier testing, adjust back to PowerUpBase if needed
{
    [Header("Clone Settings")]
    public GameObject clonePrefab;
    public int cloneCount = 3;
    public float cloneDuration = 5f;
    [Range(0f, 1f)] public float cloneTransparency = 0.4f;
    public Color cloneColor = new Color(0f, 0.5f, 1f, 1f); // Bright Blue

    [Header("Movement Settings")]
    public float cloneSpeed = 15f;
    public float angleSpread = 30f; // The angle for the Left and Right clones

    public void ActivatePower(GameObject player)
    {
        // We will spawn exactly 3 clones going in 3 different ways
        for (int i = 0; i < cloneCount; i++)
        {
            // 1. CALCULATE DIRECTION (3 WAYS)
            float angleOffset = 0f;
            if (i == 0) angleOffset = -angleSpread; // Left Way
            if (i == 1) angleOffset = 0f;           // Center Way
            if (i == 2) angleOffset = angleSpread;  // Right Way

            Vector3 spawnPos = player.transform.position + player.transform.forward * 1.5f;
            Quaternion spawnRot = Quaternion.Euler(0f, player.transform.eulerAngles.y + angleOffset, 0f);

            GameObject clone = Instantiate(clonePrefab, spawnPos, spawnRot);

            // 2. MAKE POLICE CHASE THEM
            clone.tag = "Player";

            // 3. APPLY BLUE HOLOGRAM MATERIAL
            ApplyHologramEffect(clone);

            // 4. ADD MOVEMENT
            clone.AddComponent<CloneMover>().Initialize(cloneSpeed, cloneDuration);

            // Auto-clean up
            Destroy(clone, cloneDuration);
        }
    }

    void ApplyHologramEffect(GameObject clone)
    {
        Renderer[] renderers = clone.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            // Create a new temporary material for the hologram look
            Material mat = new Material(Shader.Find("Standard"));

            // Set Color and Alpha
            mat.SetColor("_Color", new Color(cloneColor.r, cloneColor.g, cloneColor.b, cloneTransparency));

            // Make it Glow (Emission)
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", cloneColor * 2.0f); // Bright blue glow

            // Set to Transparent Mode
            mat.SetFloat("_Mode", 3); // 3 is the value for Transparent in Unity Standard Shader
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = 3000;

            r.material = mat;
        }

        // Disable physical collisions so they pass through walls/cars
        foreach (Collider col in clone.GetComponentsInChildren<Collider>())
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePower(other.gameObject);
            Destroy(gameObject);
        }
    }
}

// ------------------------
// Improved Clone Mover
// ------------------------
public class CloneMover : MonoBehaviour
{
    private float speed;
    private float duration;

    public void Initialize(float cloneSpeed, float lifeTime)
    {
        speed = cloneSpeed;
        duration = lifeTime;
    }

    void Update()
    {
        // Move automatically in the direction they were spawned
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Optional: Ghostly hover/wobble
        float wobble = Mathf.Sin(Time.time * 5f) * 0.1f;
        transform.position += Vector3.up * wobble * Time.deltaTime;
    }
}