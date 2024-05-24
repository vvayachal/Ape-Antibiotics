using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer sfxMixer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    } 

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
