using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] int sceneNumber;

    public void LoadNewScene(int sceneNumber)
    {
        if (sceneNumber == 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Debug.Log("Scene changing...");
        LoadNewScene(sceneNumber);
    }
}
