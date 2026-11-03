using UnityEngine;
using System.Collections;

public class FlightPowerUpHandler : MonoBehaviour
{
    // Change this to the Parent object that holds all your car models (Car_1 in your hierarchy)
    public GameObject carModelsParent;
    public GameObject planeVisual;
    public MonoBehaviour carController;
    public PlaneController planeController;

    public float flightTime = 8f;
    public float landingSpeed = 3f;
    public float planeHeight = 5f;

    private GameObject currentActiveCar; // To store which car was active
    bool isFlying = false;

    void Start()
    {
        planeVisual.SetActive(false);
        planeController.enabled = false;
    }

    public void ActivateFlight()
    {
        if (isFlying) return;
        StartCoroutine(FlightRoutine());
    }

    IEnumerator FlightRoutine()
    {
        isFlying = true;

        // Get the collider on this object (the car's main physics body)
        Collider playerCollider = GetComponent<Collider>();

        // 1. FIND THE ACTIVE CAR
        foreach (Transform child in carModelsParent.transform)
        {
            if (child.gameObject.activeSelf && child.gameObject != planeVisual)
            {
                currentActiveCar = child.gameObject;
                break;
            }
        }

        // 2. DISABLE PHYSICS COLLISIONS
        if (playerCollider != null) playerCollider.enabled = false; // Police will now pass through

        // 3. SWITCH TO PLANE
        planeVisual.transform.position = currentActiveCar.transform.position + Vector3.up * planeHeight;
        planeVisual.transform.rotation = currentActiveCar.transform.rotation;

        if (currentActiveCar != null) currentActiveCar.SetActive(false);
        carController.enabled = false;
        planeVisual.SetActive(true);
        planeController.enabled = true;

        yield return new WaitForSeconds(flightTime);

        // 4. LANDING
        planeController.StartLanding();
        while (planeVisual.transform.position.y > 0.5f)
        {
            planeVisual.transform.position += Vector3.down * landingSpeed * Time.deltaTime;
            yield return null;
        }

        // 5. SWITCH BACK & RE-ENABLE COLLISIONS
        planeVisual.SetActive(false);
        if (currentActiveCar != null) currentActiveCar.SetActive(true);
        carController.enabled = true;

        if (playerCollider != null) playerCollider.enabled = true; // Police can hit you again

        isFlying = false;
    }
}