using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CooldownManager : MonoBehaviour
{
    [System.Serializable]
    public class CooldownIcon
    {
        public Image icon; // The main icon image
        public Image backgroundIcon; // The darker background icon
        public float cooldownDuration = 5f; // Duration of the cooldown
    }

    public CooldownIcon[] cooldownIcons;

    public void StartCooldown(CooldownIcon cooldownIcon)
    {
        // Reset the fill amount
        cooldownIcon.icon.fillAmount = 1;

        // Animate the fill amount from 1 to 0 over the duration
        cooldownIcon.icon.DOFillAmount(0, cooldownIcon.cooldownDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Reset fill amount to 1 after cooldown is complete
            cooldownIcon.icon.fillAmount = 1;
            Debug.Log("Cooldown complete and reset");
        });
    }

    // Example method to manually start a cooldown for a specific icon
    public void TriggerCooldown(int index)
    {
        if (index >= 0 && index < cooldownIcons.Length)
        {
            StartCooldown(cooldownIcons[index]);
        }
    }
}
