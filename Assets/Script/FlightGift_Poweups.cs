using UnityEngine;

public class FlightGift_Powwerups : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FlightPowerUpHandler>().ActivateFlight();
            Destroy(gameObject);
        }
    }
}
