using Assets._Scripts.Managers;
using Assets.Resources.Entities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// Manages the game behavior
/// </summary>
public class GameManager : StaticInstance<GameManager>
{
    /// <summary>
    /// Invoked before state would change to new one
    /// </summary>
    public static event Action<GameState> OnBeforeStateChanged;

    /// <summary>
    /// Invoked after state would change to new one
    /// </summary>
    public static event Action<GameState> OnAfterStateChanged;

    /// <summary>
    /// Current game state
    /// </summary>
    public GameState State { get; private set; }


    /// <summary>
    /// Reference to player
    /// </summary>
    public static GameObject Player;

    /// <summary>
    /// game map in numeric representation
    /// </summary>
    public static int[,] map;

    /// <summary>
    /// ?????
    /// </summary>
    public static Vector2[,] mapPositions;

    /// <summary>
    /// List of all enemies alive
    /// </summary>
    public static List<GameObject> enemies;

    /// <summary>
    /// Contains info about wave name
    /// </summary>
    public Text waveName;

    /// <summary>
    /// Contains info about amount of enemies alive
    /// </summary>
    public Text enemyCounter;

    /// <summary>
    /// ?????
    /// </summary>
    public List<GameObject> gameObjects;


    /// <summary>
    /// ?????
    /// </summary>
    public GameObject uiObject;

    /// <summary>
    /// ?????
    /// </summary>
    public GameObject pauseMenuObject;

   

    /// <summary>
    /// GameObject used for a fade-in effect when scenes are changed
    /// </summary>
    public GameObject fadePanel;

    /// <summary>
    /// Value used for selecting a chapter scriptableObject displayed in levelUpUI
    /// </summary>
    public int currentChapterIndex = 0;

    /// <summary>
    /// flag that shows if the Scores were saved
    /// </summary>
    public bool ScoresWasSaved = false;

    public static bool firstLevel = true;

    public static bool gamePaused = false;

    private List<HighScore> highScores;
    private HighScore highScore;

    [SerializeField]
    private intSO scoreSO;

    [SerializeField]
    private stringSO mageNameSO;

    private Scene _currentScene;

    public CharacterStatsUI levelUpUI;


    void Start()
    {
        currentChapterIndex = 0;
        enemies = new List<GameObject>();
        highScores = new List<HighScore>(); // Initialize highScores here
        var _ = StartCoroutine(LoadScoresAsync());
        ChangeState(GameState.Hub);
        Instance.pauseMenuObject.SetActive(false);
    }


    IEnumerator LoadScoresAsync()
    {
        while (XMLManager.Instance == null)
        {
            yield return null;
        }

        highScores = XMLManager.Instance.LoadScores();
    }

    /// <summary>
    /// Method that allows to manage current game state
    /// </summary>
    /// <param name="newState">new game state</param>
    /// <exception cref="ArgumentOutOfRangeException">if the new state is not handled</exception>
    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Hub:
                HandleHub();
                break;
            case GameState.ChangeLevel:
                HandleLevelChange();
                break;
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Restarting:
                HandleStarting();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            case GameState.PostLevel:
                HandlePostLevel();
                break;
            case GameState.BossReached:
                HandleBossReached();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        ObjectPool.ClearPools();
        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    void HandleHub()
    {
        fadePanel.SetActive(true);
        var _ = StartCoroutine(LoadAsync("LevelHub", GameState.Null));

        Player = UnitManager.Instance.SpawnHero(mageNameSO.String, new Vector2(27, 42));

        highScore = new() { score = 0 };
    }
    IEnumerator LoadAsync(string SceneName, GameState state)
    {
        var loadScene = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadScene.isDone);

        _currentScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(_currentScene);

        if (state != GameState.Null)
            ChangeState(state);
    }
    void HandleLevelChange()
    {
        ObjectPool.ReturnAllObjects();
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
         fadePanel.SetActive(true);
        SceneManager.UnloadScene("LevelHub");

        var _ = StartCoroutine(LoadAsync("LevelTest", GameState.Starting));
    }



    void HandleStarting()
    {
        waveName.text = "";
        FindObjectOfType<LevelGenerator>().GenerateMap();
        PrepareLevel(30); 
        firstLevel = false;
    }

    void HandlePostLevel()
    {
        UpdateUIForLevelUp();

        ObjectPool.ReturnAllObjects();
        waveName.text = "";
        FindObjectOfType<LevelGenerator>().GenerateMap();
        PrepareLevel(10);
        currentChapterIndex++; 
    }

    private static int levelSetIndex = 0;
    void HandleBossReached()
    {
        UpdateUIForLevelUp();
        waveName.text = "";
        FindObjectOfType<LevelGenerator>().GenerateMap();
        PrepareLevel(10, true);
        levelSetIndex++; 
    }

    private void PrepareLevel(int enemyCount, bool isBossLevel = false)
    {
        if (isBossLevel)
        { 
            var bossType = GetBossTypeForLevelSet(levelSetIndex);
            UnitManager.Instance.SpawnEnemy(bossType, 1);
        }
        else
        {    
            var enemyIdStart = GetEnemyRangeStartForLevelSet(levelSetIndex);
            var enemyIdEnd = GetEnemyRangeEndForLevelSet(levelSetIndex);

            for (int i = 0; i < enemyCount; i++)
            {
                UnitManager.Instance.SpawnEnemy((ExampleEnemyType)Random.Range(enemyIdStart, enemyIdEnd + 1), 1);
            }
        }

        Player.transform.position = UnitManager.Instance.GetPlayerSpawner();
    }

    private int GetEnemyRangeStartForLevelSet(int levelSetIndex)
    {
        switch (levelSetIndex)
        {
            case 0: return 0;
            case 1: return 3;
            case 2: return 6;
            default: return 0; // Default or error handling
        }
    }

    private int GetEnemyRangeEndForLevelSet(int levelSetIndex)
    {
        switch (levelSetIndex)
        {
            case 0: return 2;
            case 1: return 5;
            case 2: return 8;
            default: return 2; // Default or error handling
        }
    }

    private ExampleEnemyType GetBossTypeForLevelSet(int levelSetIndex)
    {
        switch (levelSetIndex)
        {
            case 0: return ExampleEnemyType.OgreKing;
            case 1: return ExampleEnemyType.SpiderWitch;
            case 2: return ExampleEnemyType.CrystalDeamon;
            default: return ExampleEnemyType.OgreKing; //  error handling
        }
    }

    void HandleLose()
    {
        firstLevel = true;
        waveName.text = "YOU DIED!";
        WaveManager.Instance.StopAllCoroutines();

        highScore.score = scoreSO.Int;
        highScores.Add(highScore);
        scoreSO.Int = 0;

        ObjectPool.DestroySpellsAll();
        ObjectPool.ClearPools();

        levelSetIndex = 0;
        currentChapterIndex = 0; 

        var temp = StartCoroutine(PostLoseWait(3));
    }


    IEnumerator PostLoseWait(int delay)
    {
        var end = Time.time + delay;

        while (Time.time < end)
        {
            yield return null;
        }

        waveName.text = "Press L To Start";
        Destroy(WaveManager.Instance.gameObject);

        LevelChangeToHub();
        ChangeState(GameState.Hub);
    }

    void LevelChangeToHub()
    {
        ObjectPool.DestroyAllPooledObjects();
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        fadePanel.SetActive(true);
        enemies.Clear();

        foreach (Transform children in UnitManager.Instance.transform)
        {
            Destroy(children.gameObject);
        }
        

        SceneManager.UnloadScene("LevelTest");
    }

    /// <summary>
    /// Returns value of hight scores or new list if null
    /// </summary>
    /// <returns></returns>
    public List<HighScore> GetHightScores()
    {
        return highScores ??= new List<HighScore>();
    }

    protected override void OnApplicationQuit()
    {
        var _ = StartCoroutine(WaitToSave());
    }

    IEnumerator WaitToSave()
    {
        while (!ScoresWasSaved)
        {
            yield return null;
        }

        base.OnApplicationQuit();
    }

    public static void HandlePause() //pause not working when levelUpUi
    {
        bool isLevelUpUIActive = Instance.levelUpUI != null && Instance.levelUpUI.isActiveAndEnabled;

        if (isLevelUpUIActive)
        {

            if (gamePaused)
            {
                Instance.pauseMenuObject.SetActive(false);
            }
            else
            {
                Instance.pauseMenuObject.SetActive(true);
            }

        }
        else
        {

            gamePaused = !gamePaused;
            Time.timeScale = gamePaused ? 0f : 1f;


            Instance.pauseMenuObject.SetActive(gamePaused);
            if (!gamePaused)
            {
                Instance.uiObject.SetActive(true);
            }
        }
    }





    #region Menu

    public void HandleMenuHubReturn()
    {
        HandlePause();

        levelSetIndex = 0;
        currentChapterIndex = 0;
        firstLevel = true;
        waveName.text = "";
        WaveManager.Instance.StopAllCoroutines();

        highScore.score = scoreSO.Int;
        highScores.Add(highScore);
        scoreSO.Int = 0;

        waveName.text = "Press L To Start";

        Destroy(WaveManager.Instance.gameObject);
        ObjectPool.DestroyAllPooledObjects();
        LevelChangeToHub();
        ChangeState(GameState.Hub);
    }

    public void HandleMenuQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void HandleMenuSettings()
    {
        //OPENS SETTINGS UI 
    }
    public void HandleMenuRestart()
    {
        HandlePause();
        ChangeState(GameState.Starting);
    }
    #endregion

    public void ConfirmLevelUpAndContinue()
    {

        if (levelUpUI != null)
        {
            levelUpUI.HideUI();
        }
        else
        {
            Debug.LogError("LevelUpUI reference not set in the GameManager.");
        }


        ResumeGame();
        fadePanel.gameObject.SetActive(true);

    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
    }
    private void UpdateUIForLevelUp()
    {
        if (levelUpUI != null)
        {
            levelUpUI.ShowUI();
        }
        else
        {
            Debug.LogError("LevelUpUI reference not set in the GameManager.");
        }
    }



    void ActivateAI()
    {
        foreach(var enemy in enemies)
        {
           // enemy.activateAI();
        }

    }

}