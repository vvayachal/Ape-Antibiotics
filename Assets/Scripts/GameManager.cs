using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public static GameManager Instance;
    public Settings settings;

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
            JSONHandler.SaveData(new PlayerData(player.GetComponent<PlayerShield>().GetShieldHealth()));
        }

        // Load Game on pressing L
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData playerData = new PlayerData(0.0f);

            JSONHandler.LoadData(playerData);

            player.GetComponent<PlayerShield>().SetShieldHealth(playerData.shieldHealth);
        }
    }
}
