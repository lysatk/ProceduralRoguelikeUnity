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

        bool _canSpawn = true;
        bool _bossLevel = false;
        bool _bossSpawned = false;
        public bool gameOver = false;

        int scaleMultiplier = 1;
        //Level currentLevel;
        int currentLevelNumber = 1;
        float nextSpawnTime = 0;
        readonly float spawnInterval = 0.1f;
        int allEnemiesToSpawn;
        int spawnCountNow;
        int totalEnemies;
        int currentLevelNum = 1;

        private void Start()
        {
            waveName = GameManager.Instance.waveName;
            enemyCounter = GameManager.Instance.enemyCounter;
            waveName.text = "";
        }
        private void Update()
        {
            if (!gameOver && GameManager.map != null)
            {
                totalEnemies = GameManager.enemies.Count;
                enemyCounter.text = "Enemies Left: \n" + totalEnemies;
                // Debug.Log(currentLevel.levelName);
            }

            Debug.Log(currentLevelNum);
            if (totalEnemies == 0 && !GameManager.firstLevel)
            {
                LevelGenerator.SetTileIndex(currentLevelNum/2, FindObjectOfType<LevelGenerator>());
                currentLevelNum++;
                if (currentLevelNum % 2 == 0)  //2 is temporary for testing purpouses !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    GameManager.Instance.ChangeState(GameState.BossReached);
                else
                    GameManager.Instance.ChangeState(GameState.PostLevel);
            }

        }
    }
}
