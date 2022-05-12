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
    [SerializeField] private GameObject movementTxt;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject introTxt;
    [SerializeField] private Button backBtn;
    
    public void SetLevelNum(int lvlNum)
    {
        levelNumTxt.text = lvlNum.ToString();
    }

    public void BackToStartMenu()
    {
        LevelManager.instance.tokenSource.Cancel();
        SceneManager.LoadScene(0);
    }

    public void HandleMovementTutorial(bool state)
    {
        playerCanvas.SetActive(state);
        movementTxt.SetActive(state);
    }

    public void HandleFakesIntro(bool state)
    {
        introTxt.SetActive(state);
    }
}
