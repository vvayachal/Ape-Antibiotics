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

        [HideInInspector]
        public float cooldownDuration; // Duration of the cooldown
    }

    public CooldownIcon[] cooldownIcons;

    public void StartCooldown(CooldownIcon cooldownIcon)
    {
        // Reset the fill amount
        cooldownIcon.icon.fillAmount = 0;

        // Animate the fill amount from 1 to 0 over the duration
        cooldownIcon.icon.DOFillAmount(1, cooldownIcon.cooldownDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Reset fill amount to 1 after cooldown is complete
            cooldownIcon.icon.fillAmount = 1;
            Debug.Log("Cooldown complete and reset");
        });
    }

    // Example method to manually start a cooldown for a specific icon
    public void TriggerCooldown(int index, float time)
    {
        if (index >= 0 && index < cooldownIcons.Length)
        {
            cooldownIcons[index].cooldownDuration = time;
            StartCooldown(cooldownIcons[index]);
        }
    }
}
