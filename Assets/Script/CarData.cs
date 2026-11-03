using UnityEngine;

[System.Serializable]
public class CarData
{
    [Header("Car Info")]
    public string carId;        // unique ID (ex: CAR_0)
    public string carName;

    [Header("Unlock")]
    public int price;           // coins needed to unlock

    [Header("Model")]
    public GameObject carModel;
}
