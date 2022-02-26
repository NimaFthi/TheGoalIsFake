using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Toggle mute;
    [SerializeField] private Toggle colorCam;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private GameObject settingsPnl;

    private void Start()
    {
        _musicSlider.value = SoundManager.Instance._bgVolume;
        _sfxSlider.value = SoundManager.Instance._sfxVolume;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void MusicSliderValueChange(float value)
    {
        SoundManager.Instance.SetBgVolume(value);
    }     
    public void SfxSliderValueChange(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }

    public void MuteAudio()
    {
        SoundManager.Instance.Mute(mute.isOn);
    }

    public void ToggleCamera()
    {
        SoundManager.Instance.cameraColor = colorCam.isOn;
    }

    public void SettingPnl()
    {
        settingsPnl.SetActive(!settingsPnl.activeInHierarchy);
    }
}
