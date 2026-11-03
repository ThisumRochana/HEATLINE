using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour
{
    public CarData[] cars;

    [Header("Spawn")]
    public Transform carSpawnPoint;

    [Header("UI")]
    public TMP_Text carNameText;
    public TMP_Text priceText;
    public TMP_Text coinText;

    public GameObject unlockButton;
    public GameObject selectButton;
    public GameObject lockedPanel;

    private int currentIndex = 0;
    private GameObject currentCar;

    void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SELECTED_CAR", 0);
        LoadCar();
    }

    void LoadCar()
    {
        if (currentCar != null)
            Destroy(currentCar);

        if (cars == null || cars.Length == 0 || cars[currentIndex].carModel == null)
        {
            Debug.LogError("Car model is missing in CarData!");
            return;
        }

        currentCar = Instantiate(cars[currentIndex].carModel, carSpawnPoint);
        currentCar.transform.localPosition = Vector3.zero;
        currentCar.transform.localRotation = Quaternion.identity;

        SetLayerRecursively(currentCar, carSpawnPoint.gameObject.layer);

        Rigidbody rb = currentCar.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        UpdateGarageUI();
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void UpdateGarageUI()
    {
        CarData car = cars[currentIndex];
        carNameText.text = car.carName;
        coinText.text = PlayerPrefs.GetInt("COINS", 0).ToString();

        // 1. Check if the car is unlocked
        bool unlocked = PlayerPrefs.GetInt(car.carId, 0) == 1 || currentIndex == 0;

        // 2. Hide or Show the price based on unlock status
        if (unlocked)
        {
            priceText.text = ""; // Clears the text
            // Alternatively: priceText.gameObject.SetActive(false); 
        }
        else
        {
            priceText.text = car.price.ToString();
            // Alternatively: priceText.gameObject.SetActive(true);
        }

        lockedPanel.SetActive(!unlocked);
        unlockButton.SetActive(!unlocked);
        selectButton.SetActive(unlocked);
    }

    public void NextCar()
    {
        currentIndex = (currentIndex + 1) % cars.Length;
        LoadCar();
    }

    public void PreviousCar()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = cars.Length - 1;
        LoadCar();
    }

    public void UnlockCar()
    {
        int coins = PlayerPrefs.GetInt("COINS", 0);
        int price = cars[currentIndex].price;

        if (coins >= price)
        {
            coins -= price;
            PlayerPrefs.SetInt("COINS", coins);
            PlayerPrefs.SetInt(cars[currentIndex].carId, 1);
            PlayerPrefs.Save();
            UpdateGarageUI();
        }
    }

    public void SelectCar()
    {
        PlayerPrefs.SetInt("SELECTED_CAR", currentIndex);
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Add this to GarageManager.cs
    public void ResetAllLocks()
    {
        // Loop through all cars and set their unlock status to 0 (locked)
        foreach (var car in cars)
        {
            PlayerPrefs.SetInt(car.carId, 0);
        }

        // Optional: Reset coins if you want to test the purchase flow again
        // PlayerPrefs.SetInt("COINS", 5000); 

        PlayerPrefs.Save();
        UpdateGarageUI(); // Refresh the UI immediately
        Debug.Log("All cars locked!");
    }

}