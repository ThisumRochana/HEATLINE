using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    public LevelData[] levels;

    [Header("UI References")]
    public Image levelImage;
    public TMP_Text levelNameText;
    public GameObject lockIcon;
    public Button playButton;
    public string mainMenuSceneName = "MainMenu";

    [Header("Animation Settings")]
    public RectTransform levelDisplayContainer;
    public float transitionSpeed = 10f;

    private int index = 0;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = levelImage.rectTransform.localScale;
        UpdateUI();
    }

    void UpdateUI()
    {
        levelImage.sprite = levels[index].levelIcon;
        levelNameText.text = levels[index].levelName;

        // Check progress from PlayerPrefs
        int unlocked = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 1);
        bool isUnlocked = (index + 1) <= unlocked;

        lockIcon.SetActive(!isUnlocked);
        playButton.interactable = isUnlocked;

        StopAllCoroutines();
        StartCoroutine(AnimateTransition());
    }

    public void Next()
    {
        index = (index + 1) % levels.Length;
        UpdateUI();
    }

    public void Previous()
    {
        index--;
        if (index < 0) index = levels.Length - 1;
        UpdateUI();
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(levels[index].sceneName);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // --- NEW RESET FUNCTION ---
    public void ResetLevels()
    {
        // Resets progress to Level 1 (Practice)
        PlayerPrefs.SetInt("UNLOCKED_LEVEL", 1);
        PlayerPrefs.Save();

        // Reset the view to the first level
        index = 0;
        UpdateUI();

        Debug.Log("✅ All Heatline levels have been locked except Practice!");
    }

    IEnumerator AnimateTransition()
    {
        levelImage.rectTransform.localScale = originalScale * 0.5f;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;
            levelImage.rectTransform.localScale = Vector3.Lerp(levelImage.rectTransform.localScale, originalScale, t);
            yield return null;
        }
        levelImage.rectTransform.localScale = originalScale;
    }
}