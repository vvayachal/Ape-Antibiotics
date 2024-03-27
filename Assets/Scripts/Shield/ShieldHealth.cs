using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealth : MonoBehaviour
{
    public float shieldHealth;
    private Text shieldText;
    private Slider shieldBar;
    private float totalEntityShield;
    private Shield shield;

    private void Start()
    {
        shieldText = GetComponent<Text>();
        shieldBar = GetComponent<Slider>();
        shield = FindObjectOfType<Shield>().GetComponent<Shield>();
        totalEntityShield = shield.shieldHealth;
    }

    private void Update()
    {
        shieldText.text = shieldHealth.ToString();
        shieldBar.value = shieldHealth / totalEntityShield;
    }
}
