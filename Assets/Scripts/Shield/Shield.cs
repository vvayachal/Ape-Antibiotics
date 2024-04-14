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

    public bool cracked => shieldHealth <= 0;

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

        SelfInflictDamage();
    }


    public void TakeDamage(float damage)
    {
        if (!cracked)
        {
            shieldHealth -= damage;
        }

        if (cracked)
        {
            Debug.Log("Shield Cracked");
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
