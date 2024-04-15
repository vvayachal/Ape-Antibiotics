using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] int sceneNumber;

    public void LoadNewScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != CompareTag("Player")) return;

        LoadNewScene(sceneNumber);
    }
}
