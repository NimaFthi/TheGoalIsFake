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

    public void Save()
    {
        PlayerPrefs.SetInt("StartLevel",LevelManager.instance.currentLevel);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        LevelManager.instance.startLevel = PlayerPrefs.GetInt("StartLevel");
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("StartLevel",0);
        PlayerPrefs.Save();
    }
}