using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpPad : MonoBehaviour
{
    [SerializeField] public bool jump = true;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float upwardsForce;

    [SerializeField] private float jumpCooldown;

    private void OnCollisionEnter(Collision collision)
    {
        if (jump && collision.gameObject.CompareTag("Player"))
        {
             StartCoroutine(Jump(collision.collider, gameObject.transform.position, upwardsForce));
        }
    }

    public IEnumerator Jump (Collider player, Vector3 knockbackPoint, float knockbackDamage)
    {
        if (jump)
        {
            Debug.Log("Jump");

            Rigidbody rb = player.gameObject.GetComponentInParent<Rigidbody>();
            
            rb.AddExplosionForce(knockbackDamage * jumpMultiplier, knockbackPoint, knockbackDamage, upwardsForce);

            jump = false;
            yield return new WaitForSeconds(jumpCooldown);
            jump = true;
        }
    }
}
