using UnityEngine;

public class ShieldPower : MonoBehaviour
{
    public float shieldDuration = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarControl player = other.GetComponent<CarControl>();
            if (player != null)
            {
                player.ActivateShield(shieldDuration);
            }

            Destroy(gameObject); // remove gift box
        }
    }

}
