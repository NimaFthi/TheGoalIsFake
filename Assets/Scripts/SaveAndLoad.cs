using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private static SaveAndLoad _instance;

    public static SaveAndLoad instance
    {
        private set => _instance = value;

        get
        {
            if (_instance != null)
                return _instance;

            _instance = GameObject.FindObjectOfType<SaveAndLoad>();
            return instance;
        }
    }

    private void Awake()
    {
        var saveLoadNum = FindObjectsOfType<SaveAndLoad>().Length;
        if (saveLoadNum != 1)
        {
            Destroy(this.gameObject);
        }
        // if more then one singleton is in the scene
        //destroy ourselves
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AutoSave()
    {
        PlayerPrefs.SetInt("StartLevel", LevelManager.instance.currentLevel);
        PlayerPrefs.SetFloat("LastSavedTime", LevelManager.instance.timer);
        PlayerPrefs.Save();
    }

    public void SaveHighScore()
    {
        if (LevelManager.instance.timer < GameManager.instance.bestTimeScore)
        {
            PlayerPrefs.SetFloat("BestTimeScore", LevelManager.instance.timer);
            GameManager.instance.bestTimeScore = LevelManager.instance.timer;
        }

        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        GameManager.instance.bestTimeScore = PlayerPrefs.GetFloat("BestTimeScore");
    }

    public void AutoLoad()
    {
        LevelManager.instance.startLevel = PlayerPrefs.GetInt("StartLevel");
        LevelManager.instance.lastSavedTime = PlayerPrefs.GetFloat("LastSavedTime");
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("StartLevel", 0);
        PlayerPrefs.SetFloat("LastSavedTime", 0f);
        PlayerPrefs.Save();
    }
}