using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    
    [SerializeField] private GameObject player;
    
    [Tooltip("Reference to the Particle Effect for Speed Lines.")]
    public ParticleSystem cameraLines;
    
    public static GameManager Instance;
    private void Awake()
    {
       if(Instance == null)
       {
            Instance = this;
       }
    
       if (!cameraLines)
       {
           Debug.LogWarning("Please add reference to Camera Lines.");
       }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        cameraLines.Stop();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerShield>().IsDead)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
        
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
