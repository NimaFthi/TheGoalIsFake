using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Transform> playerSpawnPos = new List<Transform>();

    public int currentLevel;
    private int numberOfLevels;

    private void Start()
    {
        numberOfLevels = playerSpawnPos.Count;
    }
}