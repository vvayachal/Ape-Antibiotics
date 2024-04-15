using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSlam : MonoBehaviour
{
    /**
     * really this script is just the punch attack but on a heavier cooldown and damage level,
     * the main thing that will differentiate it will be animations.
     */
    private Animator anim;
    private Knockback knockb;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    float lastfired;
    public float FireRate = 30f;
    public float damage = 10f;



    void Start()
    {
        anim = GetComponent<Animator>();
        knockb = GetComponent<Knockback>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartSlam();
        }
    }

    public void StartSlam()
    {
        if (Time.time - lastfired > 1 / FireRate)
        {
            lastfired = Time.time;

            // Play attack animation
            anim.SetTrigger("Slam");

            // Detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Damage them
            foreach (Collider enemy in hitEnemies)
            {
                // pretty inefficient because we're using
                // getcomponent twice but idk how to really optimize this
                Debug.Log($"{this.name} has slammed {enemy.name}");
                if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
                }

                // Knock them back
                StartCoroutine(knockb.ApplyKnockBack(enemy, attackPoint.position, damage));
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
