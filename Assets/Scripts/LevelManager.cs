using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager instance
    {
        private set => _instance = value;

        get
        {
            if (_instance != null)
                return _instance;

            _instance = GameObject.FindObjectOfType<LevelManager>();
            return _instance;
        }
    }

    //components
    public CancellationTokenSource tokenSource;
    public NavMeshSurface navMeshSurface;
    [SerializeField] private AdManager adManager;
    [SerializeField] private Animator camAnimator;
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private GameObject enemyDieVfx;
    [SerializeField] private GameObject goalDieVfx;
    [SerializeField] private GameObject firstGuideEffect;
    [SerializeField] private GameObject secondGuideEffect;
    public Camera mainCam;

    //tutorial and level
    public bool isTutorial = true;
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;
    public List<Level> levels;

    //fakes
    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    private GameObject currentFakeEnemy;
    private GameObject currentFakeGoal;
    [SerializeField] private int delayBetweenSpawningCharacters = 1000;

    //camera
    public bool colorCam;

    //ad
    public int deathAdCountdown;
    public int levelPassAdCountdown;

    private void OnEnable()
    {
        PlayerManager.instance.OnFirstTouchToGoal += OnFirstTouchToGoal;
        PlayerManager.instance.OnSecondTouchToGoal += OnSecondTouchToGoal;
        PlayerManager.instance.OnPlayerDeath += OnPLayerDeath;
    }

    private void OnDisable()
    {
        PlayerManager.instance.OnFirstTouchToGoal -= OnFirstTouchToGoal;
        PlayerManager.instance.OnSecondTouchToGoal -= OnSecondTouchToGoal;
        PlayerManager.instance.OnPlayerDeath -= OnPLayerDeath;
    }

    private void Start()
    {
        // adManager.RequestInterstitial();
        colorCam = GameManager.instance.colorCam;

        numberOfLevels = levels.Count;
        SaveAndLoad.instance.Load();
        currentLevel = startLevel;
        levelUI.SetLevelNum(currentLevel);
        levels[startLevel].StartLevel();

        navMeshSurface.BuildNavMesh();
        PlayerManager.instance.EnablePlayer();
        if (isTutorial && currentLevel == 0)
        {
            MovementAndIntroTutorial();
            return;
        }

        isTutorial = false;
        SpawnFakes();
    }

    #region Event Functions

    private void OnFirstTouchToGoal()
    {
        SoundManager.instance.BGPlayHard();
        if (colorCam)
        {
            camAnimator.SetBool("Color", true);
        }

        if (!isTutorial) return;
        GoalIsFakeTutorial();
    }

    private void OnPLayerDeath()
    {
        SoundManager.instance.BGPlayLight();
        if (colorCam)
        {
            camAnimator.SetBool("Color", false);
        }
    }

    private void OnSecondTouchToGoal()
    {
        SoundManager.instance.BGPlayLight();
        if (currentLevel == numberOfLevels - 1)
        {
            GameManager.instance.LoadEndMenu();
            return;
        }

        if (colorCam)
        {
            camAnimator.SetBool("Color", false);
        }

        PlayerManager.instance.DisablePlayer();
        levels[currentLevel].ExitLevel();
        DestroyFakes();
        currentLevel++;
        levelUI.SetLevelNum(currentLevel);
        SaveAndLoad.instance.Save();
        levels[currentLevel].StartLevel();
        ShowInterstitialAdOnLevelPass();
        
        if (!isTutorial) return;
        isTutorial = false;
        levelUI.HandleGoalIsFakeTutorial(false);
        secondGuideEffect.SetActive(false);
    }

    #endregion

    #region Fakes

    public async void SpawnFakes()
    {
        tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;

        await Task.Delay(1, cancellationToken);
        PlayerManager.instance.canMove = false;

        await Task.Delay(delayBetweenSpawningCharacters, cancellationToken);
        currentFakeGoal = Instantiate(fakeGoalPrefab, levels[currentLevel].fakeGoalStartPos);
        currentFakeGoal.transform.localPosition = Vector3.zero;
        if (isTutorial)
        {
            var fakeGoal = currentFakeGoal.GetComponent<FakeGoal>();
            fakeGoal.goalCanvas.SetActive(true);
        }

        await Task.Delay(delayBetweenSpawningCharacters, cancellationToken);
        currentFakeEnemy = Instantiate(fakeEnemyPrefab, levels[currentLevel].fakeEnemySpawnPos);
        currentFakeEnemy.transform.localPosition = Vector3.zero;
        if (isTutorial)
        {
            var fakeEnemy = currentFakeEnemy.GetComponent<FakeEnemy>();
            fakeEnemy.enemyCanvas.SetActive(true);
        }

        PlayerManager.instance.canMove = true;
    }

    private void DestroyFakes()
    {
        EndLevelVFX();
        Destroy(currentFakeEnemy);
        Destroy(currentFakeGoal);
    }

    private void EndLevelVFX()
    {
        var enemyDieEffect = Instantiate(enemyDieVfx, currentFakeGoal.transform.position, Quaternion.identity);
        enemyDieEffect.transform.SetParent(levels[currentLevel].transform);
        Destroy(enemyDieEffect, 2f);
        var goalDieEffect = Instantiate(goalDieVfx, currentFakeEnemy.transform.position, Quaternion.identity);
        goalDieEffect.transform.SetParent(levels[currentLevel].transform);
        Destroy(goalDieEffect, 2f);
    }

    #endregion

    #region Tutorial

    private async void MovementAndIntroTutorial()
    {
        tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;
        levelUI.HandleMovementTutorial(true);

        await Task.Delay(10000, cancellationToken);

        levelUI.HandleMovementTutorial(false);
        levelUI.HandleFakesIntro(true);
        firstGuideEffect.SetActive(true);
        PlayerManager.instance.EnablePlayer();
        SpawnFakes();
    }

    private async void GoalIsFakeTutorial()
    {
        tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;

        levelUI.HandleFakesIntro(false);
        firstGuideEffect.SetActive(false);
        levelUI.HandleGoalIsFakeTutorial(true);
        secondGuideEffect.SetActive(true);
        Time.timeScale = 0;

        await Task.Delay(2000, cancellationToken);

        Time.timeScale = 1;
    }

    #endregion

    #region Ad

    public void ShowInterstitialAdOnDeath()
    {
        if (deathAdCountdown < adManager.deathNumberToShowAd - 1)
        {
            deathAdCountdown++;
            return;
        }

        deathAdCountdown = 0;
        // adManager.RequestInterstitial();
        // adManager.ShowInterstitialAd();
    }
    private void ShowInterstitialAdOnLevelPass()
    {
        if (levelPassAdCountdown < adManager.numberOfLevelPassedToShowAd - 1)
        {
            levelPassAdCountdown++;
            return;
        }

        levelPassAdCountdown = 0;
        // adManager.RequestInterstitial();
        // adManager.ShowInterstitialAd();
    }

    #endregion
}