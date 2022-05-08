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
    
    private SaveAndLoad saveAndLoad;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        saveAndLoad = GetComponent<SaveAndLoad>();
    }

    public void LoadEndMenu()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("EndMenu").buildIndex);
        saveAndLoad.ResetSave();
    }
}
