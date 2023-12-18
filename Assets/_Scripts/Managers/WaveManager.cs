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
    public class WaveManager : StaticInstance<WaveManager>
    {
        /// <summary>
        /// name of the current wave
        /// </summary>
        public Text waveName;

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
            waveName = GameManager.Instance.waveName;
            enemyCounter = GameManager.Instance.enemyCounter;
            waveName.text = "";
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
