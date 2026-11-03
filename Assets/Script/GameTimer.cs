using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float requiredTime = 60f;
    public LevelCompleteManager levelCompleteManager;

    private bool completed = false;

    [Header("UI")]
    public TextMeshProUGUI timeText;

    [Header("Level Settings")]
    public int levelNumber = 1;
    public float requiredSurviveTime = 60f; // seconds to unlock next level

    private float gameTime;
    private bool levelCompleted = false;

    void Update()
    {
        if (completed) return;

        gameTime += Time.deltaTime;
        UpdateUI();

        if (gameTime >= requiredTime)
        {
            completed = true;
            levelCompleteManager.TriggerLevelComplete();
        }
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    void CompleteLevel()
    {
        levelCompleted = true;

        int unlockedLevel = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 1);

        if (levelNumber >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UNLOCKED_LEVEL", levelNumber + 1);
            PlayerPrefs.Save();
        }

        Debug.Log("✅ Level " + levelNumber + " completed by survival time!");
    }

    public float GetTime()
    {
        return gameTime;
    }
}