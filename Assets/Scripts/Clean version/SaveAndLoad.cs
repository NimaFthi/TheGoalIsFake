using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
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