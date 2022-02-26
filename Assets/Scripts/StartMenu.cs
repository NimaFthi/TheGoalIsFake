using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Toggle mute;
    [SerializeField] private Toggle colorCam;
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
        SoundManager.Instance.Mute(mute);
    }

    public void ColorCam()
    {
        GameManager.Instance.colorCam = colorCam;
    }
}
