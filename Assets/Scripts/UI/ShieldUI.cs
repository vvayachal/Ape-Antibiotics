using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// I renamed the class to better describe its functionality [Tegomlee]
public class ShieldUI : MonoBehaviour
{
    [Tooltip("The text component attached to the Shield UI.")]
    [SerializeField] Text shieldText;

    [Tooltip("The image that represents the Shield UI.")]
    [SerializeField] Image shieldBar;

    private void Awake()
    {
        // Subscribe to events
        PlayerShield.ShieldChangedEvent += UpdateShield;
    }

    private void UpdateShield(float currentHealth, float maxHealth)
    {
        shieldText.text = $"Shield: {Mathf.Round(currentHealth)}";
        shieldBar.fillAmount = currentHealth / maxHealth;
    }
}
