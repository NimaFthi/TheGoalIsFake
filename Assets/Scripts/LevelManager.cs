using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
    public NavMeshSurface navMeshSurface;
    [SerializeField] private Animator camAnimator;
    [SerializeField] private LevelUI levelUI;
    [SerializeField] private GameObject enemyDieVfx;
    [SerializeField] private GameObject goalDieVfx;
    public Camera mainCam;


    public bool isTutorial = true;
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;
    public List<Level> levels;

    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    private GameObject currentFakeEnemy;
    private GameObject currentFakeGoal;
    [SerializeField] private int delayBetweenSpawningCharacters = 1000;
    public CancellationTokenSource tokenSource;

    public bool colorCam;

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
        colorCam = GameManager.instance.colorCam;
        SaveAndLoad.instance.ResetSave();

        numberOfLevels = levels.Count;
        SaveAndLoad.instance.Load();
        currentLevel = startLevel;
        levelUI.SetLevelNum(currentLevel);
        levels[startLevel].StartLevel();

        navMeshSurface.BuildNavMesh();
        PlayerManager.instance.EnablePlayer();
        if (isTutorial && currentLevel == 0)
        {
            Tutorial();
            return;
        }
        
        isTutorial = false;
        SpawnFakes();
    }

    private void OnFirstTouchToGoal()
    {
        SoundManager.instance.BGPlayHard();
        if (!colorCam) return;
        camAnimator.SetBool("Color", true);
    }

    private void OnPLayerDeath()
    {
        SoundManager.instance.BGPlayLight();
        if (!colorCam) return;
        camAnimator.SetBool("Color", false);
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
        
        if(!isTutorial) return;
        isTutorial = false;
        levelUI.HandleFakesIntro(false);
    }

    public async void SpawnFakes()
    {
        tokenSource = new CancellationTokenSource();
        var spawnCancellationToken = tokenSource.Token;
        
        await Task.Delay(1,spawnCancellationToken);
        PlayerManager.instance.canMove = false;
        
        await Task.Delay(delayBetweenSpawningCharacters,spawnCancellationToken);
        currentFakeGoal = Instantiate(fakeGoalPrefab, levels[currentLevel].fakeGoalStartPos);
        currentFakeGoal.transform.localPosition = Vector3.zero;
        if (isTutorial)
        {
            var fakeGoal = currentFakeGoal.GetComponent<FakeGoal>();
            fakeGoal.goalCanvas.SetActive(true);
        }

        await Task.Delay(delayBetweenSpawningCharacters,spawnCancellationToken);
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

    private async void Tutorial()
    {
        levelUI.HandleMovementTutorial(true);

        await Task.Delay(TimeSpan.FromSeconds(10));

        levelUI.HandleMovementTutorial(false);
        levelUI.HandleFakesIntro(true);
        PlayerManager.instance.EnablePlayer();
        SpawnFakes();
    }
}