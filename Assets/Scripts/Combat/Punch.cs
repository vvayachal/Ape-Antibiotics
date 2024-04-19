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
    public float damage = 10f;
    


    void Start()
    {
        anim = GetComponent<Animator>();
        knockb = GetComponent<Knockback>();
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

            // Play attack animation
            anim.SetTrigger("Punch");

            // Detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Damage them
            foreach(Collider enemy in hitEnemies)
            {
                // pretty inefficient because we're using
                // getcomponent twice but idk how to really optimize this
                Debug.Log($"{this.name} has punched {enemy.name}");
                if (enemy.gameObject.GetComponent<EnemyManager>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyManager>().TakeDamage(damage);
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
