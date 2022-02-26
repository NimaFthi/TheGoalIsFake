using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup master;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField] private AudioMixerGroup _bgGroup;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _bgAudioSource;
    [SerializeField] private SFXSoundData[] sfxSoundData;
    [Range(0f, 1f)] public float _bgVolume;
    [Range(0f, 1f)] public float _sfxVolume;

    private static SoundManager _instance;

    public static SoundManager Instance
    {
        private set => _instance = value;
        get
        {
            if (_instance != null)
                return _instance;
            _instance = GameObject.FindObjectOfType<SoundManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _bgVolume = 0.5f;
        _sfxVolume = 0.5f;
    }

    public void PlaySFX(SFXSounds sound)
    {
        foreach (var data in sfxSoundData)
        {
            if (data.SoundKey == sound)
            {
                _sfxAudioSource.PlayOneShot(data.AudioClip);
            }
        }
    }

    public void BGPlayHard()
    {
        _bgAudioSource.pitch = Mathf.Lerp(_bgAudioSource.pitch, 1.6f, .1f);
    }  
    public void BGPlayLight()
    {
        _bgAudioSource.pitch = Mathf.Lerp(_bgAudioSource.pitch, 1f, .1f);
    }

    public void SetSfxVolume(float value)
    {
        _sfxGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        _sfxVolume = value;
    }

    public void SetBgVolume(float value)
    {
        _bgGroup.audioMixer.SetFloat("BGVolume", Mathf.Log10(value) * 20);
        _bgVolume = value;
    }

    public void Mute(bool toggle)
    {
        if (toggle)
            master.audioMixer.SetFloat("MasterVolume", -80);
        else
            master.audioMixer.SetFloat("MasterVolume", 0);
    }
}

public enum SFXSounds
{
    Walk,
    Die,
    Spawn,
    Spike,
    Saw,
    SuddenSaw
}

public enum BGSounds
{
    Light,
    Hard
}

[Serializable]
public class SFXSoundData
{
    public SFXSounds SoundKey;
    public AudioClip AudioClip;
}

[Serializable]
public class BGSoundData
{
    public BGSounds SoundKey;
    public AudioClip AudioClip;
}