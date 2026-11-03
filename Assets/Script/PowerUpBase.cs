using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    public float powerDuration = 5f; // How long the power lasts

    // This will run when the player collects it
    public void Activate(GameObject player)
    {
        ApplyPower(player);
    }

    protected abstract void ApplyPower(GameObject player);
    protected abstract void RemovePower(GameObject player);
}
