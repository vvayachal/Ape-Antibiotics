using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// I renamed the class to better describe its functionality [Tegomlee]
public class ShieldUI : MonoBehaviour
{
    private Text shieldText;
    private Image shieldBar;

    private void Awake()
    {
        // Subscribe to events
        PlayerShield.ShieldChangedEvent += UpdateShield;

        // Set references
        shieldText = GameObject.Find("ShieldText").GetComponent<Text>();
        shieldBar = GameObject.Find("ShieldBar").GetComponent<Image>();
    }

    private void UpdateShield(float currentHealth, float maxHealth)
    {
        shieldText.text = $"Shield: {Mathf.Round(currentHealth)}";
        shieldBar.fillAmount = currentHealth / maxHealth;
    }
}
