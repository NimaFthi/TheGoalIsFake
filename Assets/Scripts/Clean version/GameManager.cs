using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{
    private static NewGameManager _instance;

    public static NewGameManager instance
    {
        private set => _instance = value;

        get
        {
            if (_instance != null)
                return _instance;

            _instance = GameObject.FindObjectOfType<NewGameManager>();
            return _instance;
        }
    }

    public bool colorCam;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadEndMenu()
    {
        SceneManager.LoadScene(2);
        SaveAndLoad.instance.ResetSave();
    }
}
