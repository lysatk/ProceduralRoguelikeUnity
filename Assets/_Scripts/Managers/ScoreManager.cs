using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class ScoreManager : StaticInstance<ScoreManager>
    {
        private const string HighScoreKey = "HighScore";
        private const int MaxScores = 5;
        /// <summary>
        /// Value of a current score player got in the current run, saved only on win
        /// </summary>
        public int currentScore=0;

        /// <summary>
        /// Adds the passed value to the current score
        /// </summary>
        /// <param name="score"></param>
        public void AddValueToScore(int score)
        {
            currentScore += score;
        }

        /// <summary>
        /// Saves the current score and updates the high scores
        /// </summary>
        /// <param name="score"></param>
        public void SaveScore()
        {
            UpdateHighScores(currentScore);
            currentScore = 0;
        }
        /// <summary>
        /// Calculates the time based addition to the score and saves it 
        /// </summary>
        /// <param name="score"></param>
        public int CalculateScoreAndSave(float startTime, float endTime)
        {
            int score = CalculateScoreBasedOnTime(startTime, endTime);
            currentScore += score;
            SaveScore();
            return score;
        }

        private int CalculateScoreBasedOnTime(float startTime, float endTime)
        {
            float durationInMinutes = (endTime - startTime) / 60.0f; 
            int scoreDeduction = Mathf.FloorToInt(durationInMinutes) * 10; 
            int baseScore = 1000;
            int finalScore = baseScore - scoreDeduction;
            return Mathf.Max(finalScore, 0); 
        }


        private void UpdateHighScores(int newScore)
        {
            for (int i = 0; i < MaxScores; i++)
            {
                string key = HighScoreKey + i;
                int storedScore = PlayerPrefs.GetInt(key, 0);
                if (newScore > storedScore)
                {
                    for (int j = MaxScores - 1; j > i; j--)
                    {
                        PlayerPrefs.SetInt(HighScoreKey + j, PlayerPrefs.GetInt(HighScoreKey + (j - 1), 0));
                    }
                    PlayerPrefs.SetInt(key, newScore);
                    break; 
                }
            }
            PlayerPrefs.Save();
        }


        public int LoadHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey + "0", 0);
        }

        public int[] GetTopScores()
        {
            int[] topScores = new int[MaxScores];
            for (int i = 0; i < MaxScores; i++)
            {
                topScores[i] = PlayerPrefs.GetInt(HighScoreKey + i, 0);
            }
            return topScores;
        }
    }
}
