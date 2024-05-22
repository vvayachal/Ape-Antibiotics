using System;

[Serializable]
public class PlayerData
{
    public float shieldHealth;
    public int score;
    
    public PlayerData(float shieldHealth, int score)
    {
        this.shieldHealth = shieldHealth;
        this.score = score;
    }
}
