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
    private SaveAndLoad saveAndLoad;

    public bool firstTime = true;
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;
    public List<Level> levels;

    [SerializeField] private GameObject fakeEnemyPrefab;
    [SerializeField] private GameObject fakeGoalPrefab;
    private GameObject currentFakeEnemy;
    private GameObject currentFakeGoal;


    private void OnEnable()
    {
        PlayerManager.instance.OnSecondTouchToGoal += OnSecondTouchToGoal;
    }

    private void OnDisable()
    {
        PlayerManager.instance.OnSecondTouchToGoal -= OnSecondTouchToGoal;
    }

    private void Start()
    {
        saveAndLoad = GetComponent<SaveAndLoad>();
        numberOfLevels = levels.Count;

        saveAndLoad.Load();
        currentLevel = startLevel;
        
        levels[startLevel].StartLevel();
        navMeshSurface.BuildNavMesh();
        PlayerManager.instance.EnablePlayer();
        SpawnFakes();
    }

    private void OnSecondTouchToGoal()
    {
        if (currentLevel == numberOfLevels - 1)
        {
            NewGameManager.instance.LoadEndMenu();
            return;
        }
        PlayerManager.instance.DisablePlayer();
        levels[currentLevel].ExitLevel();
        currentLevel++;
        saveAndLoad.Save();
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