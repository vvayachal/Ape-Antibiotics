using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    public Settings settings;
    
    public static GameManager Instance;
    private void Awake()
    {
       if(Instance == null)
       {
            Instance = this;
       }
    }

    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Save Game on pressing Y
        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            SaveData();
        }

        // Load Game on pressing L
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    public Vector3 getPlayerPosition()
    {
        return player.transform.position;
    }
    
    private void SaveData()
    {
        JSONHandler.SaveData(new PlayerData(player.GetComponent<PlayerShield>().GetShieldHealth(), ScoreManager.Instance.GetScore()));
    }
    
    private void LoadData()
    {
        PlayerData playerData = new PlayerData(0.0f, 0);

        JSONHandler.LoadData(playerData);

        player.GetComponent<PlayerShield>().SetShieldHealth(playerData.shieldHealth);
        ScoreManager.Instance.SetScore(playerData.score);       
    }
    
    
}
