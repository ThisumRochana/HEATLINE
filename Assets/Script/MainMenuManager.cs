using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // These names must match your scene names exactly
    [Header("Scene Names")]
    public string levelSelectScene = "LevelSelect";
    public string garageScene = "GarageScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void OpenGarage()
    {
        SceneManager.LoadScene(garageScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            // For WebGL, we usually redirect to a URL or close the tab
            Application.ExternalEval("window.close();"); 
            // Note: window.close() only works if the script opened the window. 
            // Often, WebGL games just redirect back to the home page:
            // Application.OpenURL("https://yourwebsite.com");
#else
            Application.Quit();
#endif
    }
}