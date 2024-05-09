using System;
using UnityEngine;

public class PlayerShield : MonoBehaviour, IDamageable
{
    [Tooltip("The maximum value the shield has.")]
    [SerializeField] float maxShieldHealth = 100f;

    //----------

    // Variables
    private float currentShieldHealth;
    private bool isCracked = false;
    public bool IsCracked {  get { return isCracked; } }

    // Events
    public static event Action<float, float> ShieldChangedEvent;

    //----------

    [Header("Debugging")]
    [SerializeField] KeyCode inflictDamageKey = KeyCode.T;

    //----------

    private void Awake()
    {
        // Set the current health
        currentShieldHealth = maxShieldHealth;

        // Invoke the event
        ShieldChangedEvent?.Invoke(currentShieldHealth, maxShieldHealth);
    }

    private void Update()
    {
        // Debugging
        SelfInflictDamage();
    }


    public void TakeDamage(float damage)
    {
        // Take damage
        currentShieldHealth -= damage;

        // Check for state
        if (currentShieldHealth <= 0f) 
        {
            currentShieldHealth = 0f;
            isCracked = true;
        }
        else
        {
            isCracked = false;
        }

        // Invoke the event
        ShieldChangedEvent?.Invoke(currentShieldHealth, maxShieldHealth);

        // Debugging
        if (isCracked)
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
