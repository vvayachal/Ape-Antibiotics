using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] int sceneNumber;
    [SerializeField] TextMeshProUGUI enemiesToDefeatUI;
    [SerializeField] TextMeshProUGUI enemiesDefeatedUI;
    [SerializeField] TextMeshProUGUI itemsToDestroyUI;
    [SerializeField] TextMeshProUGUI itemsDestroyedUI;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
        }
    }

    private void Update()
    {
        if (enemiesToDefeatUI != null)
        {
            enemiesToDefeatUI.text = enemiesToDefeat.ToString();
        }
        if (enemiesDefeatedUI != null)
        {
            enemiesDefeatedUI.text = enemiesDefeated.ToString();
        }
        if (itemsToDestroyUI != null)
        {
            itemsToDestroyUI.text = itemsToDestroy.ToString();
        }
        if (itemsDestroyedUI != null)
        {
            itemsDestroyedUI.text = itemsDestroyed.ToString();
        }
    }

    // Singleton 
    //public static GameManager Instance { get; private set; }

    // Variables to track player's progress
    private int enemiesDefeated = 0;
    private int itemsDestroyed = 0;

    // Objectives to complete the game
    public int enemiesToDefeat = 10;
    public int itemsToDestroy = 5;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    // Call when an enemy is defeated
    public void EnemyDefeated()
    {
        enemiesDefeated++;
        Debug.Log("Enemies deafeated: " + enemiesDefeated);
        CheckGameEnd();
    }

    // Call when an item is collected
    public void ItemDestroyed()
    {
        itemsDestroyed++;
        Debug.Log("Items Destroyed: " + itemsDestroyed);
        CheckGameEnd();
    }

    // Check if the game should end
    private void CheckGameEnd()
    {
        if (enemiesDefeated >= enemiesToDefeat && itemsDestroyed >= itemsToDestroy)
        {
            EndGame();
        }
    }

    // End the game
    private void EndGame()
    {
        Debug.Log("Game Completed!");

        GameObject.FindGameObjectWithTag("WinScreen").GetComponent<Canvas>().enabled = true;
    }

    // Methods to get progress (optional)
    public int GetEnemiesDefeated()
    {
        return enemiesDefeated;
    }

    public int GetItemsCollected()
    {
        return itemsDestroyed;
    }
}
