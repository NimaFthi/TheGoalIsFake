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

    public bool isTutorial = true;
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;
    public List<Level> levels;

    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    private GameObject currentFakeEnemy;
    private GameObject currentFakeGoal;
    [SerializeField] private float delayBetweenSpawningCharacters = 1f;

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
        colorCam = NewGameManager.instance.colorCam;
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
            NewGameManager.instance.LoadEndMenu();
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
    }

    public async void SpawnFakes()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1));
        PlayerManager.instance.canMove = false;
        
        await Task.Delay(TimeSpan.FromSeconds(delayBetweenSpawningCharacters));
        currentFakeGoal = Instantiate(fakeGoalPrefab, levels[currentLevel].fakeGoalStartPos);
        currentFakeGoal.transform.localPosition = Vector3.zero;

        await Task.Delay(TimeSpan.FromSeconds(delayBetweenSpawningCharacters));
        currentFakeEnemy = Instantiate(fakeEnemyPrefab, levels[currentLevel].fakeEnemySpawnPos);
        currentFakeEnemy.transform.localPosition = Vector3.zero;
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
        SpawnFakes();
    }
}