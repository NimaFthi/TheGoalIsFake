using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager instance
    {
        private set => _instance = value;

        get
        {
            if (_instance != null)
                return _instance;

            _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    //setting
    public bool colorCam;
    
    //Score
    public float bestTimeScore = Mathf.Infinity;

    private void Awake()
    {
        var gameManagersNum = FindObjectsOfType<GameManager>().Length;
        if (gameManagersNum != 1)
        {
            Destroy(this.gameObject);
        }
        
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        
        SaveAndLoad.instance.LoadHighScore();
    }

    private void OnApplicationQuit()
    {
        if(SceneManager.GetActiveScene().buildIndex != 1) return;
        
        SaveAndLoad.instance.AutoSave();
    }

    public void LoadEndMenu()
    {
        SaveAndLoad.instance.SaveHighScore();
        SceneManager.LoadScene(2);
        SaveAndLoad.instance.ResetSave();
    }
}
