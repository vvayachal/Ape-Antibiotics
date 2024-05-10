using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;

    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(instance);
        }
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;
    [SerializeField] private AudioMixer MusicMixer;
    [SerializeField] private AudioMixer SFXMixer;

    //============================= UI Manager ====================================
    // Start is called before the first frame update
    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
    }

    public void ShowSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //=========================== Settings ===================================
    public void SetMusicVolume(float volume)
    {
        MusicMixer.SetFloat("volume", volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        SFXMixer.SetFloat("volume", volume);
    }

    //============================= Scene ====================================
    public void LoadNewScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
}
