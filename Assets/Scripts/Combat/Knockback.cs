using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [SerializeField] public bool knockback = true;
    [SerializeField] float knockbackMultiplier = 0.05f;
    [SerializeField] float upwardsForce = 10f;

    float timeToReposition = 2f;

    private void Start()
    {
        timeToReposition = FindObjectOfType<EnemyManager>().timeBeforeReposition;
    }

    public IEnumerator ApplyKnockBack (Collider enemy, Vector3 knockbackPoint, float knockbackDamage)
    {
        // Add use cases for components that don't exist. 
        // Some knockbackable objects wont have nav, etc. 

        if (knockback)
        {
            enemy.GetComponent<EnemyBehaviour>().isKnockedBack = true;
            //Debug.Log("knockback applied");

            var nav = enemy.GetComponent<NavMeshAgent>();
            var anim = enemy.GetComponent<Animator>();
            var rb = enemy.GetComponent<Rigidbody>();
            var en = enemy.GetComponent<EnemyManager>();

            anim.enabled = false;
            nav.enabled = false;
            en.enabled = false;
            rb.AddExplosionForce(knockbackDamage * knockbackMultiplier, knockbackPoint, knockbackDamage, upwardsForce);
            //Debug.Log("Knockback Force: " + knockbackDamage * knockbackMultiplier + ", Upwards Force: " + upwardsForce);

            yield return new WaitForSeconds(timeToReposition);

            if (en != null)
            {
                en.enabled = true;
            }
            if (anim != null)
            {
                anim.enabled = true;
            }
            if (nav != null)
            {
                nav.enabled = true;
            }

            enemy.GetComponent<EnemyBehaviour>().isKnockedBack = false;
        }
    }
}
