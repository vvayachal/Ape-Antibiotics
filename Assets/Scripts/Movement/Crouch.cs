using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private bool Crouching { get => isCrouching; }
    private PlayerMovement _playerMovement;
    
    public bool isCrouching;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            
        }
    }
}
