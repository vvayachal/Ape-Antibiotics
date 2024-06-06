using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    // All of there variables can be refactored into a scriptable object to save on memory [Tegomlee]

    [Tooltip("Set to True if script is attached to the player.")] [SerializeField]
    private bool isPlayer;
    
    [Tooltip("The point where the attack originates from.")]
    [SerializeField] Transform attackPoint;

    [Tooltip("The size of the collision hitbox for the attack.")]
    [SerializeField] float attackRange = 0.5f;

    [Tooltip("The layers affected by the attack.")]
    [SerializeField] LayerMask affectedLayers;

    [Tooltip("The cooldown of the attack.")]
    [SerializeField] float attackCooldown = 1f;

    [Tooltip("The minimum amount of damage the attack does.")]
    [SerializeField] float minDamage = 10f;

    [Tooltip("The maximum amount of damage the attack does.")]
    [SerializeField] float maxDamage = 30f;

    [Tooltip("The increment to the damage the enemy attack does to the player.")] [SerializeField]
    private float damageIncrement = 0;

    [Tooltip("How fast (in seconds) the damage reaches its maximum.")]
    [SerializeField] float chargeUpTime;

    [Tooltip("How long (in seconds) can the player maintain the charge before it automatically punches.")]
    [SerializeField] float chargeHoldTime;

    [Tooltip("The amount of knockback applied to the target.")]
    [SerializeField] float attackKnockbackForceMultiplier;
    
    [HideInInspector] public bool triggerPunch = false;

    //----------

    // References
    private Animator anim;

    // Variables
    private bool canPunch = true;
    private bool isChargingUp = false;
    
    private WaitForSeconds punchCooldownSeconds;

    private float damageBasedOnCharge;
    private float timeSinceChargeUpStarted = 0f;
    
    //----------

    void Awake()
    {
        // Assign the components
        anim = GetComponent<Animator>();

        // Assign the coroutine duration
        punchCooldownSeconds = new WaitForSeconds(attackCooldown);

        // Assign damageBasedOnCharge
        damageBasedOnCharge = minDamage;
    }

    void Update()
    {
        if (canPunch)
        {
            // Checks whether the player can begin charging up
            if (isPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Set the states
                    canPunch = false;
                    isChargingUp = true;

                    ChargeUpPunch();

                    // Fire the charge up event as true;
                    EventManager.InvokeChargeUpEvent(isChargingUp);
                }

                if (Input.GetMouseButtonUp(0) && isChargingUp)
                {
                    PerformPunch();
                }
            }
            else
            {
                if (triggerPunch)
                {
                    // Set the states
                    canPunch = false;
                   

                    PerformPunch();

                    // Fire the charge up event as true;
                    EventManager.InvokeChargeUpEvent(isChargingUp);
                }
            }
        }
    }
    //TODO: Implement the chargeUpEvent to this script to fire the events.
    //TODO: Implement a ChargeUpMethod.
    //TODO: Change StartPunch To PerformPunch and call the coroutine at the end of this method instead.

    private void ChargeUpPunch()
    {
        if (isChargingUp)
        {
            // Increase the time since the charge up started
            timeSinceChargeUpStarted += Time.deltaTime;

            // Calculate the charge progress
            float chargeProgress = Mathf.Clamp01(timeSinceChargeUpStarted / chargeUpTime);

            // Calculate the damage based on charge progress
            damageBasedOnCharge = Mathf.Lerp(minDamage, maxDamage, chargeProgress);

            // Check to see whether the player has held the charge up for too long
            if (timeSinceChargeUpStarted >= (chargeUpTime + chargeHoldTime))
            {
                PerformPunch();
            }
        }
    }

    private void PerformPunch()
    {
        // Reset isCharging
        isChargingUp = false;

        // Play attack animation
        anim.SetTrigger("Punch");

        // Detect enemies in range of attack
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position,
                                                        attackRange,
                                                        affectedLayers);

        // Damage them
        foreach (Collider target in hitTargets)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this

            // Refactored to use TryGetComponent instead, Could further refactor with an interface or abstract class [Tegomlee]
            Debug.Log($"{this.name} has punched {target.name}");
            
            // Knock them back - Now uses the IKnockable interface
            if (isPlayer)
            {
                if (target.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
                {
                    enemyHealth.TakeDamage(damageBasedOnCharge);
                }
                
                if (target.gameObject.TryGetComponent<IKnockable>(out var knockable))
                {
                    // Knocks the enemy based on multiple factors
                    float finalKnockbackValue = attackKnockbackForceMultiplier * damageBasedOnCharge;
                    knockable.KnockBack(attackPoint.position, finalKnockbackValue);
                }
            }
            else
            {
                PlayerShield playerShield = target.gameObject.GetComponentInParent<PlayerShield>();
                
                if (playerShield)
                {
                    playerShield.TakeDamage(damageBasedOnCharge);
                }
                
                
                IKnockable knockable = target.gameObject.GetComponentInParent<IKnockable>();
                
                if (knockable != null)
                {
                    // Knocks the enemy based on multiple factors
                    float finalKnockbackValue = (attackKnockbackForceMultiplier * this.damageBasedOnCharge) > maxDamage ? maxDamage : attackKnockbackForceMultiplier * damageBasedOnCharge; 
                     
                    knockable.KnockBack(attackPoint.position, finalKnockbackValue);
                }
            }
            
        }

        StartCoroutine(PunchCoolDown());
    }

    private IEnumerator PunchCoolDown()
    {
        // Perform the cooldown
        yield return punchCooldownSeconds;

        // Clean up the states and values
        timeSinceChargeUpStarted = 0f;
        if (isPlayer)
        {
            damageBasedOnCharge = minDamage;
        }
        else
        {
            damageBasedOnCharge += damageIncrement;
        }
        canPunch = true;
    }





    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        if (attackPoint.parent.gameObject.activeSelf)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}