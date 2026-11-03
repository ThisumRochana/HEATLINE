using UnityEngine;
using UnityEngine.UI; // Required for the Slider

public class LevelProgressBar : MonoBehaviour
{
    public Slider progressBar;      // Drag your UI Slider here
    public GameObject levelCompletePanel;
    public int currentLevelNumber = 1;
    public float targetSurvivalTime = 60f; // 1 minute

    private float currentTime = 0f;
    private bool isLevelUnlocked = false;

    void Start()
    {
        levelCompletePanel.SetActive(false);
        if (progressBar != null)
        {
            progressBar.maxValue = targetSurvivalTime;
            progressBar.value = 0;
        }
    }

    void Update()
    {
        if (isLevelUnlocked) return;

        if (currentTime < targetSurvivalTime)
        {
            currentTime += Time.deltaTime;
            UpdateProgressBar();
        }
        else
        {
            UnlockNextLevel();
        }
    }

    void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = currentTime;
        }
    }

    void UnlockNextLevel()
    {
        isLevelUnlocked = true;

        // Save progress using your existing PlayerPrefs logic
        int unlockedLevel = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 1);
        if (currentLevelNumber >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UNLOCKED_LEVEL", currentLevelNumber + 1);
            PlayerPrefs.Save();
        }

        // Show the completion UI
        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ResetLevelProgress()
    {
        PlayerPrefs.SetInt("UNLOCKED_LEVEL", 1);
        PlayerPrefs.Save();
        // Reload the scene to refresh the lock icons
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}