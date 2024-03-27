using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public GameObject shieldInCanvas;
    private ShieldHealth shield;
    public float shieldHealth;

    public bool cracked;

    [Header("Keybinds")]
    [SerializeField] KeyCode inflictDamageKey = KeyCode.T;

    void Start()
    {
        shield = shieldInCanvas.GetComponentInChildren<ShieldHealth>();
    }

    void Update()
    {
        // Update shield health
        shield.shieldHealth = shieldHealth;

        ShieldCrack();

        SelfInflictDamage();
    }

    private void ShieldCrack()
    {
       if (shieldHealth > 0)
       {
           cracked = false;
       }
       else
       {
           cracked = true;
       }

       if (cracked)
        {
            // Play some crack animation
        }
    }

    public void TakeDamage(float damage)
    {
        if (!cracked)
        {
            shieldHealth -= damage;
        }
    }

    private void SelfInflictDamage()
    {
        if (Input.GetKeyDown(inflictDamageKey))
        {
            TakeDamage(10f);
        }
    }
}
