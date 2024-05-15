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
    }

    public void ShowSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadNewScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
}
