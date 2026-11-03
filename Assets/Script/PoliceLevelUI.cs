using UnityEngine;
using UnityEngine.UI;

public class PoliceLevelUI : MonoBehaviour
{
    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;

    void Start()
    {
        SetPoliceLevel(1); // default start level
    }

    public void SetPoliceLevel(int level)
    {
        // Disable all first
        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;

        if (level >= 1) star1.enabled = true;
        if (level >= 2) star2.enabled = true;
        if (level >= 3) star3.enabled = true;
    }
}
