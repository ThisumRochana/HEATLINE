using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;    // Assign the Score Text inside your Game Over Panel
    public TextMeshProUGUI survivalTimeText; // Assign the Time Text inside your Game Over Panel

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    private float survivalTimer;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Reset game state
        Time.timeScale = 1f;
        isGameOver = false;
        survivalTimer = 0f;

        if (gameOverPanel)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Keep counting time as long as the player is alive
        if (!isGameOver)
        {
            survivalTimer += Time.deltaTime;
        }
    }

    public void GameOver(float delay = 0f)
    {
        if (isGameOver) return;
        isGameOver = true;

        StopAllSirens();

        if (delay <= 0f)
        {
            ShowGameOver();
        }
        else
        {
            Invoke(nameof(ShowGameOver), delay);
        }
    }

    private void ShowGameOver()
    {
        // Pause the game physics and movement
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 1. Get the score from your ScoreManager script
        if (ScoreManager.Instance != null && finalScoreText != null)
        {
            int finalScore = ScoreManager.Instance.GetSessionScore();
            finalScoreText.text =  finalScore.ToString();
        }

        // 2. Display the survival time formatted as 00:00
        if (survivalTimeText != null)
        {
            survivalTimeText.text =  FormatTime(survivalTimer);
        }
    }

    // Helper function to turn seconds into a clean Time format
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StopAllSirens()
    {
        PoliceSirenSystem[] sirens = FindObjectsByType<PoliceSirenSystem>(FindObjectsSortMode.None);
        foreach (var siren in sirens)
        {
            siren.StopSiren();
        }
    }

    // --- BUTTON FUNCTIONS ---

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}