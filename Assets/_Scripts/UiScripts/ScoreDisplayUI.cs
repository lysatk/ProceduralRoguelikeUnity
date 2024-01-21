using UnityEngine;
using TMPro; 
using System.Collections.Generic; 
using UnityEngine.SceneManagement;

using Assets._Scripts.Managers;

public class ScoreDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText; 
    public List<TextMeshProUGUI> highScoreTexts; 

    private void Start()
    {
        DisplayCurrentScore();
        DisplayTopHighScores();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void DisplayCurrentScore()
    {
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        currentScoreText.text = "Current Score: " + currentScore.ToString();
    }

    private void DisplayTopHighScores()
    {
        int[] highScores = ScoreManager.Instance.GetTopScores();
        for (int i = 0; i < highScores.Length && i < highScoreTexts.Count; i++)
        {
            highScoreTexts[i].text = "High Score " + (i + 1) + ": " + highScores[i].ToString();
        }
    }
}
