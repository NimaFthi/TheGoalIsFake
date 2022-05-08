using System;
using System.Collections.Generic;
using System.Threading;
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

    public bool firstTime = true;
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;
    public List<Level> levels;

    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    private GameObject currentFakeEnemy;
    private GameObject currentFakeGoal;

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
        numberOfLevels = levels.Count;

        SaveAndLoad.instance.Load();
        currentLevel = startLevel;

        levels[startLevel].StartLevel();
        navMeshSurface.BuildNavMesh();
        PlayerManager.instance.EnablePlayer();
        SpawnFakes();
    }
    
    private void OnFirstTouchToGoal()
    {
        SoundManager.Instance.BGPlayHard();
        if(!colorCam) return;
        camAnimator.SetBool("Color", true);
    }

    private void OnPLayerDeath()
    {
        SoundManager.Instance.BGPlayLight();
        if(!colorCam) return;
        camAnimator.SetBool("Color", false);
    }

    private void OnSecondTouchToGoal()
    {
        SoundManager.Instance.BGPlayLight();
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
        currentLevel++;
        SaveAndLoad.instance.Save();
        DestroyFakes();
        levels[currentLevel].StartLevel();
    }

    public void SpawnFakes()
    {
        currentFakeEnemy = Instantiate(fakeEnemyPrefab, levels[currentLevel].fakeEnemySpawnPos);
        currentFakeEnemy.transform.localPosition = Vector3.zero;
        currentFakeGoal = Instantiate(fakeGoalPrefab, levels[currentLevel].fakeGoalStartPos);
        currentFakeGoal.transform.localPosition = Vector3.zero;
    }

    private void DestroyFakes()
    {
        Destroy(currentFakeEnemy);
        Destroy(currentFakeGoal);
    }
}