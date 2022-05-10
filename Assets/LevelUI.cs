using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumTxt;
    [SerializeField] private Button backBtn;
    
    public void SetLevelNum(int lvlNum)
    {
        levelNumTxt.text = lvlNum.ToString();
    }

    public void BackToStartMenu()
    {
        SceneManager.LoadScene(0);
    }
}
