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

    [Tooltip("Set the number of coins to drop on death")] 
    [SerializeField] private int coinsToDrop;

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
        scoreText.text = score.ToString();

        if (playerShield.IsCracked)
        {
            
        }
    }
    
    public void Score()
    {
        score += 1;
    }

    public void DropCoins()
    {
        if (score < coinsToDrop)
        {
            coinsToDrop = score;
        }
        score -= coinsToDrop;

        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector3 playerPosition = GameManager.Instance.getPlayerPosition();

            Vector3 spawnPosition = new Vector3(Random.Range(playerPosition.x - 3, playerPosition.x + 3), playerPosition.y, Random.Range(playerPosition.z - 3, playerPosition.z + 3));
            
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
