using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Tooltip("Layer(s) the enemy can interact with.")]
    [SerializeField] LayerMask affectedLayer;

    [Tooltip("Amount of damage the enemy does.")]
    [SerializeField] float amountOfDamage;

    [Tooltip("Amount of force added to the player.")]
    [SerializeField] float knockbackForceAmount;

    [Tooltip("Amount of additional knockback damage applied to the player when health is zero.")]
    [SerializeField] float knockoutForceMultiplier;

    [Tooltip("How much time (in seconds) must pass before the enemy can sttack again.")]
    [SerializeField] float timeBetweenDamageTicks;

    //----------

    // Variables
    private WaitForSeconds damageTickSeconds;
    private bool canDamage = true;

    //----------

    private void Awake()
    {
        // Set damageTickSeconds
        damageTickSeconds = new WaitForSeconds(timeBetweenDamageTicks);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks for the correct layer, using my custom LayerMaskExtensions; if the collider has an IDamagable component; amd if the enemy can damage
        if (LayerMaskExtensions.IsLayerInMask(affectedLayer, collision.gameObject.layer) && collision.gameObject.TryGetComponent(out IDamageable damageable) && canDamage)
        {
            // Set the damage state
            canDamage = false;

            // Apply damage to the object
            damageable.TakeDamage(amountOfDamage);
            
            // Apply knockback to the object, checking for player health
            if (damageable.CurrentHealth > 0)
            {
                Knockback.Instance.PerformKnockback(collision.collider, transform.position, knockbackForceAmount);
            }
            else
            {
                Knockback.Instance.PerformKnockback(collision.collider, transform.position, knockbackForceAmount * knockoutForceMultiplier, true);
            }

            // Start the coroutine
            StartCoroutine(ResetDamageState());
        }
    }

    private IEnumerator ResetDamageState()
    {
        yield return damageTickSeconds;

        canDamage = true;
    }
}
