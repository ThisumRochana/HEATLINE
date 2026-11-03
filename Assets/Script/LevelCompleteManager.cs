using UnityEngine;
using System.Collections;
using TMPro; // Needed for the countdown text

public class LevelCompleteManager : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public TMP_Text countdownText; // Drag a TextMeshPro object here
    public int levelNumber = 1;
    public float popupDuration = 3f;

    private bool levelCompleted = false;

    void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    public void TriggerLevelComplete()
    {
        if (levelCompleted) return;

        levelCompleted = true;
        StartCoroutine(LevelCompleteRoutine());
    }

    IEnumerator LevelCompleteRoutine()
    {
        // 1. Stop the game
        Time.timeScale = 0f;

        // 2. Unlock next level in PlayerPrefs
        UnlockNextLevel();

        // 3. Show the "Level Complete" Image/Panel
        levelCompletePanel.SetActive(true);

        // 4. Wait for the initial popup duration
        yield return new WaitForSecondsRealtime(popupDuration);

        // 5. Hide the "Level Complete" Image/Panel
        levelCompletePanel.SetActive(false);

        // 6. Start the 3, 2, 1 Countdown
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            int count = 3;
            while (count > 0)
            {
                countdownText.text = count.ToString();
                yield return new WaitForSecondsRealtime(1f);
                count--;
            }

            countdownText.text = "GO!";
            yield return new WaitForSecondsRealtime(0.5f);
            countdownText.gameObject.SetActive(false);
        }

        // 7. Resume the game
        Time.timeScale = 1f;
        levelCompleted = false; // Reset for next time if needed
    }

    void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 1);

        if (levelNumber >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UNLOCKED_LEVEL", levelNumber + 1);
            PlayerPrefs.Save();
            Debug.Log("✅ Next Level Unlocked!");
        }
    }
}