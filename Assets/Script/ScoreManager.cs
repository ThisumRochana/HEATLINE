using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;


    [Header("UI Reference")]
    public TextMeshProUGUI scoreText; // Assign your in-game Score Text here

    // Variables
    private int sessionScore = 0; // Starts at 0 (Visual only)
    private int totalCoins = 0;   // The total money in the bank

    void Awake()
    {
        // Force this new instance to take over
        Instance = this;
    }

    void Start()
    {
        // 1. Load the TOTAL coins from the database (so we don't lose previous progress)
        totalCoins = PlayerPrefs.GetInt("COINS", 0);

        // 2. Set the VISUAL score to 0 (New Game = Start from 0)
        sessionScore = 0;

        // 3. Update the text to show "0"
        UpdateUI();
    }

    // Call this when player destroys a car
    public void AddScore(int amount)
    {
        // 1. Update the Session Score (Show the player they earned points)
        sessionScore += amount;

        // 2. Update the Total Wallet (Add to the bank in the background)
        totalCoins += amount;

        // 3. Save the TOTAL to the database IMMEDIATELY
        // This ensures the Main Menu and Garage will see the new total later.
        PlayerPrefs.SetInt("COINS", totalCoins);
        PlayerPrefs.Save();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            // We display 'sessionScore', so the player sees "Score: 10... 20... 30"
            // We do NOT display 'totalCoins' here.
            scoreText.text = "Score: " + sessionScore;
        }
    }

    // Add this inside the ScoreManager class
    public int GetSessionScore()
    {
        return sessionScore;
    }
}