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

    public bool colorCam;

    private void Awake()
    {
        var gameManagersNum = FindObjectsOfType<GameManager>().Length;
        if (gameManagersNum != 1)
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

    public void LoadEndMenu()
    {
        SceneManager.LoadScene(2);
        SaveAndLoad.instance.ResetSave();
    }
}
