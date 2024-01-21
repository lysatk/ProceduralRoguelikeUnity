using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Managers
{

    /// <summary>
    /// Class that handle waves
    /// </summary>
    public class LevelManager : StaticInstance<LevelManager>
    {
        /// <summary>
        /// name of the current wave
        /// </summary>
        public Text levelName;

        /// <summary>
        /// text that contains number of enemies alive
        /// </summary>
        public Text enemyCounter;
        public bool gameOver = false;


        int totalEnemies;
        int currentLevelNum = 1;
        LevelGenerator levelGenerator;

        private void Start()
        {
            levelName = GameManager.Instance.levelName;
            enemyCounter = GameManager.Instance.enemyCounter;
            levelName.text = "";
            levelGenerator = FindObjectOfType<LevelGenerator>();
            currentLevelNum = 1;
            LevelGenerator.SetTileIndex(0,levelGenerator);
        }
        private void Update()
        {
            if (!gameOver && GameManager.map != null)
            {
                totalEnemies = GameManager.enemies.Count;
                enemyCounter.text = "Enemies Left: \n" + totalEnemies;
            }

           
            if (totalEnemies == 0 && !GameManager.firstLevel)
            {
                if (currentLevelNum == 9)
                {
                    GameManager.Instance.ChangeState(GameState.Win);
                }
                    LevelGenerator.SetTileIndex(currentLevelNum / 3, levelGenerator);
                currentLevelNum++;
                
                if (currentLevelNum % 3 == 0) 
                    GameManager.Instance.ChangeState(GameState.BossReached);
                else
                    GameManager.Instance.ChangeState(GameState.PostLevel);
            }

        }
    }
}
