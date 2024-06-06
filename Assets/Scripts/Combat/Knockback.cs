using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [SerializeField] private bool knockback = true;
    [SerializeField] private float knockbackMultiplier = 1f;
    [SerializeField] private float upwardsForce = 10f;

    private float timeToReposition;

    private void Start()
    {
        if (FindObjectOfType<EnemyHealth>() != null)
        {
            timeToReposition = FindObjectOfType<EnemyHealth>().timeBeforeReposition;
        }
    }

    public IEnumerator ApplyKnockBack (Collider enemy, Vector3 knockbackPoint, float knockbackDamage)
    {
        // Add use cases for components that don't exist. 
        // Some knockbackable objects wont have nav, etc. 

        if (knockback)
        {
            Debug.Log("knockback applied");

            var nav = enemy.GetComponent<NavMeshAgent>();
            var anim = enemy.GetComponent<Animator>();
            var rb = enemy.GetComponent<Rigidbody>();
            var en = enemy.GetComponent<EnemyMotor>();

            anim.enabled = false;
            nav.enabled = false;
            en.enabled = false;
            rb.AddExplosionForce(knockbackDamage * knockbackMultiplier, knockbackPoint, knockbackDamage, upwardsForce);

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
        }
    }
}
