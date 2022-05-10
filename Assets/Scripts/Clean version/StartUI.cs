using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Toggle mute;
    [SerializeField] private Toggle colorCam;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private GameObject settingsPnl;


    private void Awake()
    {
        ToggleCamera();
    }

    private void Start()
    {
        _musicSlider.value = SoundManager.instance._bgVolume;
        _sfxSlider.value = SoundManager.instance._sfxVolume;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MusicSliderValueChange(float value)
    {
        SoundManager.instance.SetBgVolume(value);
    }

    public void SfxSliderValueChange(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }

    public void MuteAudio()
    {
        SoundManager.instance.Mute(mute.isOn);
    }

    public void ToggleCamera()
    {
        NewGameManager.instance.colorCam = colorCam.isOn;
    }

    public void SettingPnl()
    {
        settingsPnl.SetActive(!settingsPnl.activeInHierarchy);
    }
}