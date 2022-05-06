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
        numberOfLevels = levels.Count;
        currentLevel = 0;
        foreach (var level in levels)
        {
            if (level.levelDone)
                currentLevel++;
        }

        startLevel = currentLevel;
        levels[startLevel].StartLevel();
        navMeshSurface.BuildNavMesh();
        SpawnFakes();
    }

    private void OnSecondTouchToGoal()
    {
        if (currentLevel == numberOfLevels - 1)
        {
            Debug.Log(" DONE !");
            return;
        }
        PlayerManager.instance.DisablePlayer();
        levels[currentLevel].ExitLevel();
        currentLevel++;
        DestroyFakes();
        levels[currentLevel].StartLevel();
    }

    public void SpawnFakes()
    {
        currentFakeEnemy = Instantiate(fakeEnemyPrefab, levels[currentLevel].fakeEnemySpawnPos.position,
            Quaternion.identity);
        currentFakeGoal = Instantiate(fakeGoalPrefab, levels[currentLevel].fakeGoalStartPos.position,
            Quaternion.identity);
    }

    public void DestroyFakes()
    {
        Destroy(currentFakeEnemy);
        Destroy(currentFakeGoal);
    }
}