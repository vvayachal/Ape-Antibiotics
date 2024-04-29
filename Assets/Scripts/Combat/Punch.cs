using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private Animator anim;
    private Knockback knockb;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    float lastfired;
    public float FireRate = 20f;
    
    /*
     * This is now a mulitplier isntead of the base damage
     * Tweak accordingly [Tegomlee]
     */
    public float damageMultiplier = 5f;
    // Min and Max damage values the player can make [Tegomlee]
    public float minDamageValue = 5f, maxDamageValue = 50f;

    // Rigidbody reference used for speed calculations [Tegomlee]
    private Rigidbody rb;
    


    void Start()
    {
        anim = GetComponent<Animator>();
        knockb = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPunch();
        }
    }

    public void StartPunch()
    {
        if (Time.time - lastfired > 1 / FireRate)
        {
            lastfired = Time.time;

            // Calculate the final damage for the current attack [Tegomlee]
            float calculatedDamageValue = rb.velocity.magnitude * damageMultiplier;
            float finalDamageValue;
            if (calculatedDamageValue < minDamageValue) finalDamageValue = minDamageValue;
            else if (calculatedDamageValue > maxDamageValue) finalDamageValue = maxDamageValue;
            else finalDamageValue = calculatedDamageValue;
            Debug.Log($"Final Damage: {finalDamageValue}");
            Debug.Log($"Initial Damage: {calculatedDamageValue}");

            // Play attack animation
            anim.SetTrigger("Punch");

            // Detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Damage them
            foreach(Collider enemy in hitEnemies)
            {
                // pretty inefficient because we're using
                // getcomponent twice but idk how to really optimize this
                if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(finalDamageValue);
                    Debug.Log($"{this.name} has punched {enemy.name} for {finalDamageValue} damage.");
                }

                // Knock them back
                StartCoroutine(knockb.ApplyKnockBack(enemy, attackPoint.position, finalDamageValue));
            }
        }
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
