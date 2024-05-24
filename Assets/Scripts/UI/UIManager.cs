using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //============================= UI Manager ====================================
    // Start is called before the first frame update
    private void Start()
    {
        if (mainMenu != null)
        {
            ShowMainMenu();
        }
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
}
