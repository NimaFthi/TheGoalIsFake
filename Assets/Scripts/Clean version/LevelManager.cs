using System;
using System.Collections.Generic;
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
    [SerializeField] private NavMeshSurface navMeshSurface;
    // public List<Transform> playerSpawnPos = new List<Transform>();
    // public List<Transform> fakeEnemyStartPos = new List<Transform>();
    // public List<Transform> fakeGoalStartPos = new List<Transform>();
    // public List<Transform> fakeEnemyRunAwayPos = new List<Transform>();
    
    public int currentLevel;
    public int startLevel;
    private int numberOfLevels;

    public List<Level> levels;
    
    
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
    }

    private void OnSecondTouchToGoal()
    {
        if (currentLevel != numberOfLevels - 1)
        {
            levels[currentLevel].ExitLevel();
            currentLevel++;
            levels[currentLevel].StartLevel();
            navMeshSurface.BuildNavMesh();
        }
    }
}