using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int sceneNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
        }
    }
}
