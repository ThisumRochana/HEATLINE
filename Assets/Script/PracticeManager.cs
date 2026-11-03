using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class PracticeMissionManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tutorialPanel;
    public GameObject gameOverPanel;     // Drag your "BUSTED" panel here
    public GameObject levelCompletePanel; // NEW: Drag your "SUCCESS" panel here
    public GameObject pauseMenuPanel;
    public RectTransform leftLetter;
    public RectTransform rightLetter;

    [Header("Text Displays")]
    public TextMeshProUGUI hudTimeText;
    public TextMeshProUGUI hudScoreText;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI pauseTimeText;
    public TextMeshProUGUI pauseScoreText;

    [Header("Assignments")]
    public GameObject playerObject;
    public GameObject policePrefab;
    public Transform spawnPoint;

    private bool gameStarted = false;
    private bool isGameOver = false;
    private int policeDestroyedCount = 0;
    private const int TOTAL_POLICE_TO_WIN = 3;
    private GameObject currentPoliceInstance;
    private float elapsedTime = 0f;

    private int currentScore = 0;
    private const int POINTS_PER_CAR = 10;

    void Start()
    {
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false); // Hide success panel at start
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        StartCoroutine(PulseLetters());
        SpawnPolice();
    }

    void Update()
    {
        if (!gameStarted && Keyboard.current != null)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
                BeginGame();
            else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
                BeginGame();
        }

        if (gameStarted && !isGameOver && Time.timeScale > 0)
        {
            elapsedTime += Time.deltaTime;
            UpdateUI();

            if (playerObject == null || !playerObject.activeInHierarchy)
            {
                TriggerGameOver(false);
            }
        }
    }

    void UpdateUI()
    {
        if (hudTimeText) hudTimeText.text = "Time: " + FormatTime(elapsedTime);
        if (hudScoreText) hudScoreText.text = "Score: " + currentScore;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void BeginGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            tutorialPanel.SetActive(false);
            Time.timeScale = 1f;
            StartCoroutine(CheckPoliceStatus());
        }
    }

    IEnumerator CheckPoliceStatus()
    {
        while (policeDestroyedCount < TOTAL_POLICE_TO_WIN && !isGameOver)
        {
            if (currentPoliceInstance == null)
            {
                policeDestroyedCount++;
                currentScore += POINTS_PER_CAR;

                if (policeDestroyedCount < TOTAL_POLICE_TO_WIN)
                {
                    yield return new WaitForSeconds(1.0f);
                    if (!isGameOver) SpawnPolice();
                }
                else
                {
                    // WIN PATH: All police cars destroyed
                    TriggerGameOver(true);
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void TriggerGameOver(bool didWin)
    {
        isGameOver = true;
        StopAllPoliceSirens();

        if (didWin)
        {
            // Unlock Level 1 for Azura Studio's LevelSelect system
            PlayerPrefs.SetInt("UNLOCKED_LEVEL", 2);
            PlayerPrefs.Save();

            StartCoroutine(LevelCompleteWinRoutine());
        }
        else
        {
            // BUSTED PATH: 3 second delay for the police chase feel
            Invoke("ShowEndPanelLose", 3.0f);
        }
    }

    IEnumerator LevelCompleteWinRoutine()
    {
        // 1. Freeze the game world
        Time.timeScale = 0f;

        // 2. Show the SUCCESS Panel
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        // 3. Wait for 2 seconds in real-time
        yield return new WaitForSecondsRealtime(2.0f);

        // 4. Load the Level Selection scene
        SceneManager.LoadScene("LevelSelect");
    }

    void ShowEndPanelLose()
    {
        if (finalTimeText) finalTimeText.text = FormatTime(elapsedTime);
        if (finalScoreText) finalScoreText.text = currentScore.ToString();

        gameOverPanel.SetActive(true); // Shows BUSTED panel
        Time.timeScale = 0f;
    }

    // --- BUTTON FUNCTIONS ---
    public void PauseGame()
    {
        if (pauseMenuPanel != null && !isGameOver)
        {
            if (pauseTimeText) pauseTimeText.text = "Time: " + FormatTime(elapsedTime);
            if (pauseScoreText) pauseScoreText.text = "Score: " + currentScore;
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            if (!isGameOver) Time.timeScale = 1f;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void StopAllPoliceSirens()
    {
        GameObject[] allPolice = GameObject.FindGameObjectsWithTag("Police");
        foreach (GameObject p in allPolice)
        {
            AudioSource[] sources = p.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource s in sources) { s.Stop(); s.mute = true; }
        }
    }

    void SpawnPolice() { if (policePrefab && spawnPoint) currentPoliceInstance = Instantiate(policePrefab, spawnPoint.position, spawnPoint.rotation); }

    IEnumerator PulseLetters()
    {
        while (!gameStarted)
        {
            float scale = 1f + Mathf.PingPong(Time.unscaledTime * 1.5f, 0.15f);
            if (leftLetter) leftLetter.localScale = new Vector3(scale, scale, 1);
            if (rightLetter) rightLetter.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }
}