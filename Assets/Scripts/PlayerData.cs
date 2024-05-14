using System;

[Serializable]
public class PlayerData
{
    public float shieldHealth;
    
    public PlayerData(float shieldHealth)
    {
        this .shieldHealth = shieldHealth;
    }
}
