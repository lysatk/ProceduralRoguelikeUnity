using UnityEngine;
using UnityEngine.UI; // Include the UI namespace

public class UIManager : MonoBehaviour
{
 
    public GameObject levelUpUI;
    public void ToggleLevelUpUI()
    {

        if (levelUpUI != null)
        {
            bool isActive = levelUpUI.activeSelf;

            // Toggle the active state
            levelUpUI.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("LevelUpUI reference is not set in the UIManager.");
        }
    }
}
