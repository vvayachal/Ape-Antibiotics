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

    [Tooltip("The amount of damage the attack does.")]
    [SerializeField] float damage = 10f;

    [Tooltip("The amount of knockback applied to the enemy.")]
    [SerializeField] float knockbackForce;

    //----------

    // References
    private Animator anim;

    // Variables
    private bool canPunch = true;
    private WaitForSeconds punchCooldownSeconds;

    void Awake()
    {
        // Assign the components
        anim = GetComponent<Animator>();

        // Assign the coroutine duration
        punchCooldownSeconds = new WaitForSeconds(attackCooldown);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPunch)
        {
            StartCoroutine(PunchCoolDown());
        }
    }

    private void StartPunch()
    {
        // Play attack animation
        anim.SetTrigger("Punch");

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position,
                                                        attackRange,
                                                        affectedLayers);

        // Damage them
        foreach (Collider enemy in hitEnemies)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this

            // Refactored to use TryGetComponent instead, Could further refactor with an interface or abstract class [Tegomlee]
            Debug.Log($"{this.name} has punched {enemy.name}");
            if (enemy.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
            }

            // Knock them back - Now uses the IKnockable interface
            if (enemy.gameObject.TryGetComponent<IKnockable>(out var knockable))
            {
                knockable.KnockBack(attackPoint.position, knockbackForce);
            }
        }
    }

    private IEnumerator PunchCoolDown()
    {
        // Set punch state
        canPunch = false;

        // Activate the punch
        StartPunch();

        // Perform the cooldown
        yield return punchCooldownSeconds;

        // Reset the punch state
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