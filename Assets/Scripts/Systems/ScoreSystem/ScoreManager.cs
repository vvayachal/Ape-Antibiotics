using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [Tooltip("Reference to Player Shield")] 
    [SerializeField] private PlayerShield playerShield;
    
    [Tooltip("Reference to Score Value Object.")]
    [SerializeField] private Text scoreText;

    // [Tooltip("Set the number of coins to drop on object destruction")] 
    // [SerializeField] private int coinsToDrop;
    
    [Tooltip("Set the number of coins to drop on Player Death")] 
    [SerializeField] private int coinsToLose;

    [Tooltip("Coin Prefab")] 
    [SerializeField] private GameObject coin;
    
    private int score;
    public static ScoreManager Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    
    public void Score()
    {
        score += 1;
        
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Make the player lose coins on death. Number of coins set from the inspector in Score Manager script.
    /// </summary>
    public void LoseCoins()
    {
        if (score < coinsToLose)
        {
            coinsToLose = score;
        }
        score -= coinsToLose;
        scoreText.text = score.ToString();
        
        DropCoins(coinsToLose, GameManager.Instance.getPlayerPosition());
    }
    
    /// <summary>
    /// Drops specified number of coins at specified location.
    /// </summary>
    /// <param name="coinsToDrop"> Number of coins to drop.</param>
    /// <param name="dropPosition"> Position to drop the coins at.</param>
    public void DropCoins( int coinsToDrop, Vector3 dropPosition)
    {
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(dropPosition.x - 3, dropPosition.x + 3), dropPosition.y, Random.Range(dropPosition.z - 3, dropPosition.z + 3));
            
            Instantiate(coin, spawnPosition, Quaternion.identity);
        }
    }
    
    public int GetScore()
    {
        return score;
    }

    public void SetScore(int scoreValueToSet)
    {
        score = scoreValueToSet;
    }
}
