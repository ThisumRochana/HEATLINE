using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static UnityEditor.MaterialProperty;

public class IntroMenuUI : MonoBehaviour
{
    public RectTransform title;
    public CanvasGroup tapToStart;

    void Start()
    {
        // Reset
        title.localScale = Vector3.zero;
        tapToStart.alpha = 0f;

        // Title pop animation
        title.DOScale(1f, 1f)
             .SetEase(Ease.OutBack);

        // Blinking "Tap to Start"
        tapToStart.DOFade(1f, 1f)
                  .SetLoops(-1, LoopType.Yoyo)
                  .SetDelay(1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadMainMenu();
        }
    }

    void LoadMainMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }
}
