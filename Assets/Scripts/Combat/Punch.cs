using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    // All of there variables can be refactored into a scriptable object to save on memory [Tegomlee]

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

    [Tooltip("How fast (in seconds) the damage reaches its maximum.")]
    [SerializeField] float chargeUpTime;

    [Tooltip("How long (in seconds) can the player maintain the charge before it automatically punches.")]
    [SerializeField] float chargeHoldTime;

    [Tooltip("The amount of knockback applied to the enemy.")]
    [SerializeField] float attackKnockbackForceMultiplier;

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
        anim = GetComponentInChildren<Animator>();

        // Assign the coroutine duration
        punchCooldownSeconds = new WaitForSeconds(attackCooldown);

        // Assign damageBasedOnCharge
        damageBasedOnCharge = minDamage;
    }

    void Update()
    {
        // Checks whether the player can begin charging up
        if (Input.GetMouseButtonDown(0) && canPunch)
        {
            // Set the states
            canPunch = false;
            isChargingUp = true;

            // Fire the charge up event as true;
            EventManager.InvokeChargeUpEvent(isChargingUp);
        }

        // Checks whether the player can perform the punch
        // (Allows the player to punch without having to wait for the full charge)
        if (Input.GetMouseButtonUp(0) && isChargingUp)
        {
            PerformPunch();
        }

        ChargeUpPunch();
    }

    //TODO: Implement the chargeUpEvent to this script to fire the events.
    //TODO: Implement a ChargeUpMethod.
    //TODO: Change StartPunch To PerformPunch and call the coroutine at the end of this method instead.

    private void ChargeUpPunch()
    {
        if (isChargingUp)
        {
            //Set animator 
            anim.SetBool("Charging", true);

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
        // Set animator
        anim.SetBool("Charging", false);
        anim.SetTrigger("Punching");

        // Reset isCharging
        isChargingUp = false;

        // Play attack animation
        //anim.SetTrigger("Punch");

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position,
                                                        attackRange,
                                                        affectedLayers);

        // Damage them
        foreach (Collider enemy in hitEnemies)
        {
            // Refactored to use TryGetComponent instead, Could further refactor with an interface or abstract class [Tegomlee]
            Debug.Log($"{this.name} has punched {enemy.name}");
            if (enemy.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(damageBasedOnCharge);
            }

            // Calculate the final knock force
            float finalKnockbackValue = attackKnockbackForceMultiplier * damageBasedOnCharge;

            // Prepare the enemy for knockback
            // This isn't a great solution, will refactor if necessary [Tegomlee].
            enemy.gameObject.GetComponent<EnemyMotor>().PrepareEnemyForKnockback();

            // Apply the knockback
            Knockback.Instance.PerformKnockback(enemy, attackPoint.position, finalKnockbackValue);
        }

        StartCoroutine(PunchCoolDown());
    }

    private IEnumerator PunchCoolDown()
    {
        // Perform the cooldown
        yield return punchCooldownSeconds;

        // Clean up the states and values
        timeSinceChargeUpStarted = 0f;
        damageBasedOnCharge = 0f;
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