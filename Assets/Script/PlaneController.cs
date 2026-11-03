using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlaneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 25f;
    public float turnSpeed = 60f;
    public float targetHeight = 6f;
    public float heightIncreaseSpeed = 3f;
    public float landingSpeed = 4f;

    [Header("Real Stick Feel (Visuals)")]
    public Transform planeModel;
    public float maxTiltAngle = 35f;
    public float tiltSmoothSpeed = 8f;

    [Header("Propeller Settings")]
    public Transform propeller;       // DRAG YOUR PROPELLER CHILD HERE
    public float propellerSpinSpeed = 1000f; // How fast it spins

    [HideInInspector] public bool isLanding = false;

    private Rigidbody rb;
    private float horizontalInput = 0f;
    private float currentZRotation = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleInput();
        ApplyVisualEffects();
    }

    void FixedUpdate()
    {
        // 1. Constant Forward Movement
        rb.linearVelocity = transform.forward * moveSpeed;

        // 2. Turning (Y-Axis)
        float turn = horizontalInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 3. Altitude Logic
        HandleAltitude();
    }

    void HandleInput()
    {
        horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) horizontalInput = -1f;
            if (Keyboard.current.dKey.isPressed) horizontalInput = 1f;
        }

        if ((Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) ||
            (Mouse.current != null && Mouse.current.leftButton.isPressed))
        {
            float inputX = (Touchscreen.current != null)
                ? Touchscreen.current.primaryTouch.position.ReadValue().x
                : Mouse.current.position.ReadValue().x;

            horizontalInput = (inputX < Screen.width / 2f) ? -1f : 1f;
        }
    }

    void ApplyVisualEffects()
    {
        // --- 1. TILT EFFECT ---
        if (planeModel != null)
        {
            float targetZ = -horizontalInput * maxTiltAngle;
            currentZRotation = Mathf.Lerp(currentZRotation, targetZ, Time.deltaTime * tiltSmoothSpeed);
            planeModel.localRotation = Quaternion.Euler(0, 0, currentZRotation);
        }

        // --- 2. PROPELLER ROTATION ---
        if (propeller != null)
        {
            // Rotates the propeller on its local Z axis every frame
            propeller.Rotate(Vector3.forward * propellerSpinSpeed * Time.deltaTime);
        }
    }

    void HandleAltitude()
    {
        if (!isLanding && transform.position.y < targetHeight)
        {
            rb.MovePosition(rb.position + Vector3.up * heightIncreaseSpeed * Time.fixedDeltaTime);
        }
        else if (isLanding && transform.position.y > 0.5f)
        {
            rb.MovePosition(rb.position - Vector3.up * landingSpeed * Time.fixedDeltaTime);
        }
    }

    public void StartLanding()
    {
        isLanding = true;
    }
}