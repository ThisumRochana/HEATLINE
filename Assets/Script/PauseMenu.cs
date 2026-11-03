using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject hudPauseButton;

    [Header("Display Fields")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    private bool isPaused = false;

    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1f;
        // Make sure audio is playing at start
        AudioListener.pause = false;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else ActivatePause();
    }

    public void ActivatePause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // --- THIS STOPS THE SIRENS AND ALL SOUNDS ---
        AudioListener.pause = true;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (hudPauseButton != null) hudPauseButton.SetActive(false);

        UpdateStats();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // --- THIS STARTS THE SIRENS AGAIN ---
        AudioListener.pause = false;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (hudPauseButton != null) hudPauseButton.SetActive(true);
    }

    void UpdateStats()
    {
        // 1. Handle Time
        float t = Time.timeSinceLevelLoad;
        int hours = (int)(t / 3600);
        int minutes = (int)((t % 3600) / 60);
        int seconds = (int)(t % 60);

        if (timeText != null)
            timeText.text = string.Format("TIME: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        // 2. Handle Score using your ScoreManager Instance
        if (scoreText != null && ScoreManager.Instance != null)
        {
            // We call the function you added: GetSessionScore()
            int currentScore = ScoreManager.Instance.GetSessionScore();

            // "D4" makes it look like 0005 instead of just 5
            scoreText.text = "SCORE: " + currentScore.ToString("D4");
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false; // Reset audio for the next scene
        SceneManager.LoadScene("MainMenu");
    }
}