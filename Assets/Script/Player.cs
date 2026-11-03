using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CarControl : MonoBehaviour
{
    [Header("Car Settings")]
    public float MoveSpeed = 250f;
    public float MaxSpeed = 25f;
    public float Drag = 0.98f;
    public float SteeringAngle = 20f;
    public float Traction = 1f;

    [Header("Crash FX")]
    public GameObject explosionFX; // Assign your particle here

    [Header("Speed Power")]
    private float currentSpeed;
    public TrailRenderer speedTrail; // assign in inspector

    [Header("Shield Power")]
    public GameObject shieldVisual;
    private bool shieldActive = false;


    private Vector3 moveForce;
    private bool isDead = false;

    void Start()
    {
        currentSpeed = MoveSpeed;

        // Disable trail at game start
        if (speedTrail != null)
            speedTrail.enabled = false;
    }




    void Update()
    {
        if (isDead) return;

        float steerInput = 0f;

        // Keyboard
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) steerInput = -1f;
            else if (Keyboard.current.dKey.isPressed) steerInput = 1f;
        }

        // Touch
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.isPressed)
                {
                    float touchX = touch.position.ReadValue().x;
                    steerInput = (touchX < Screen.width / 2f) ? -1f : 1f;
                }
            }
        }

        // Mouse
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            float mouseX = Mouse.current.position.ReadValue().x;
            steerInput = (mouseX < Screen.width / 2f) ? -1f : 1f;
        }

        // Move
        // Move
        moveForce += transform.forward * currentSpeed * Time.deltaTime;
        moveForce = Vector3.ClampMagnitude(moveForce, MaxSpeed * (currentSpeed / MoveSpeed)); // scale MaxSpeed with boost
        transform.position += moveForce * Time.deltaTime;


        // Steering
        transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * SteeringAngle * Time.deltaTime);

        // Drag & Traction
        moveForce *= Drag;
        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, Traction * Time.deltaTime) * moveForce.magnitude;
    }

    public void ModifySpeed(float multiplier)
    {
        currentSpeed = MoveSpeed * multiplier;

        // Enable trail when speed boost is active
        if (speedTrail != null)
            speedTrail.enabled = true;
    }

    public void ResetSpeed()
    {
        currentSpeed = MoveSpeed;

        // Disable trail when speed boost ends
        if (speedTrail != null)
            speedTrail.enabled = false;
    }

    public void ActivateShield(float duration)
    {
        if (!shieldActive)
            StartCoroutine(ShieldRoutine(duration));
    }

    IEnumerator ShieldRoutine(float duration)
    {
        shieldActive = true;
        if (shieldVisual) shieldVisual.SetActive(true);

        yield return new WaitForSeconds(duration);

        shieldActive = false;
        if (shieldVisual) shieldVisual.SetActive(false);
    }




    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Police") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (shieldActive) return; // block damage
            CrashPlayer();
        }
    }

    public void CrashPlayer()

    {
        if (isDead) return;

        isDead = true;
        moveForce = Vector3.zero;

        // Disable visuals & collider
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh) mesh.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        // Spawn explosion as separate object
        float fxDuration = 2f;
        if (explosionFX)
        {
            GameObject fx = Instantiate(explosionFX, transform.position, Quaternion.identity);
            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) fxDuration = ps.main.duration;
            Destroy(fx, fxDuration);
        }

        // Destroy player immediately
        Destroy(gameObject);

        // Trigger Game Over after explosion
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver(fxDuration);
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }
}