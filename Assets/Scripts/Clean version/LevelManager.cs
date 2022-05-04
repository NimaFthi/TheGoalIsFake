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
    public List<Transform> playerSpawnPos = new List<Transform>();
    public List<Transform> fakeEnemyStartPos = new List<Transform>();
    public List<Transform> fakeGoalStartPos = new List<Transform>();
    public List<Transform> fakeEnemyRunAwayPos = new List<Transform>();


    public int currentLevel;
    private int numberOfLevels;

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
        numberOfLevels = playerSpawnPos.Count;
    }

    private void OnSecondTouchToGoal()
    {
        if (currentLevel != numberOfLevels - 1)
        {
            
            currentLevel++;
            
        }
    }
}