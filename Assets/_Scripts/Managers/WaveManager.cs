using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Managers
{
    /// <summary>
    /// Class to store information about Levels (current game situation)
    /// </summary>
    [System.Serializable]
    public class Level
    {
        /// <summary>
        /// name of the wave
        /// </summary>
        public string levelName;

        /// <summary>
        /// number of enemies that will spawn at this wave
        /// </summary>
        public int noOfEnemies;

        /// <summary>
        /// the time of pause after wave
        /// </summary>
        public float nextSpawnTime;

        /// <summary>
        /// constructor to initiate waves
        /// </summary>
        /// <param name="waveName">name of the wave</param>
        /// <param name="noOfEnemies">number of enemies that will spawn at this wave</param>
        /// <param name="nextSpawnTime">the time of pause after wave</param>
        public Level(string waveName, int noOfEnemies, float nextSpawnTime)
        {
            this.levelName = waveName;
            this.noOfEnemies = noOfEnemies;

            if (nextSpawnTime < 30)
                this.nextSpawnTime = nextSpawnTime;
            else
                this.nextSpawnTime = 30;
        }
    }

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
        Level currentLevel;
        int currentLevelNumber = 1;
        float nextSpawnTime = 0;
        readonly float spawnInterval = 0.1f;
        int allEnemiesToSpawn;
        int spawnCountNow;
        int totalEnemies;

        private void Start()
        {
            waveName = GameManager.Instance.waveName;
            enemyCounter = GameManager.Instance.enemyCounter;
            // TrySpawn();
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


            if (totalEnemies == 0)
            {
                GameManager.Instance.ChangeState(GameState.PostLevel);
            }

        }
    }
}
        //    void TrySpawn()
        //    {
        //        if (_canSpawn && Time.time > nextSpawnTime)
        //        {
        //            currentLevel = new Level("Wave: " + currentLevelNumber, currentLevelNumber + 4, currentLevelNumber + 4);

//            if (currentLevelNumber % 5 == 0)
//            {
//                enemyCounter.text = "Enemies Left: \n" + totalEnemies;
//                allEnemiesToSpawn = 1;
//                _bossLevel = true;
//            }
//            else
//                allEnemiesToSpawn = currentLevel.noOfEnemies;

//            _ = StartCoroutine(SpawnWave());
//            scaleMultiplier++;
//            waveName.text = currentLevel.levelName;//set UI Text to waveName
//        }
//    }

//    IEnumerator SpawnWave()
//    {
//        _canSpawn = false;

//        while (allEnemiesToSpawn > 0)
//        {
//            if (!_bossLevel)
//            {
//                EnemiesToSpawnSetter(allEnemiesToSpawn);

//                for (int i = 0; i < spawnCountNow; i++)
//                {
//                    UnitManager.Instance.SpawnEnemy((ExampleEnemyType)Random.Range(0, 3), scaleMultiplier);
//                    allEnemiesToSpawn--;
//                }
//            }
//            else
//            {
//                if (!_bossSpawned)
//                {
//                    UnitManager.Instance.SpawnEnemy(ExampleEnemyType.Boss, scaleMultiplier);
//                    _bossSpawned = true;
//                }

//                if (totalEnemies == 0)
//                {
//                    allEnemiesToSpawn--;
//                    _bossLevel = false;
//                    _bossSpawned = false;
//                }
//            }

//            yield return new WaitForSeconds(spawnInterval);
//        }

//        nextSpawnTime = Time.time + currentLevel.nextSpawnTime;
//        currentLevelNumber++;
//        _canSpawn = true;
//    }

//    void EnemiesToSpawnSetter(int enemyCount)
//    {
//        if (enemyCount < 5)
//            spawnCountNow = enemyCount;
//        else
//            spawnCountNow = 5;
//    }
//}
