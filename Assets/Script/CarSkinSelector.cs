using UnityEngine;

public class CarSkinSelector : MonoBehaviour
{
    [Header("Drag all Car Models here (In Order)")]
    public GameObject[] carModels;

    void Awake() // We use Awake so this happens BEFORE the car controller starts
    {
        // 1. Get the ID of the car we selected in the Garage
        int selectedIndex = PlayerPrefs.GetInt("SELECTED_CAR", 0);

        // Safety Check: If the number is too big, go back to 0
        if (selectedIndex >= carModels.Length) selectedIndex = 0;

        // 2. Loop through all models
        for (int i = 0; i < carModels.Length; i++)
        {
            if (i == selectedIndex)
            {
                // Turn ON the selected car
                carModels[i].SetActive(true);
            }
            else
            {
                // Turn OFF all other cars
                carModels[i].SetActive(false);
            }
        }
    }
}