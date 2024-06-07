using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Destructible : MonoBehaviour, IKnockable
{
    Transform target; // Changed target to search for the player in awake [Tegomlee]
    Rigidbody rb;

    //-----------------------

    [Header("KnockBack Attributes")]

    [Tooltip("Controls wether the enemy is able to be knocked back. (Mainly for debugging purposes).")]
    [SerializeField] bool _isKnockbackable;

    [Tooltip("Controls how much knockback force is applied to this specific enemy (Lower is lighter)."), Range(0.2f, 6f)]
    [SerializeField] float _knockbackForceMultplier = 1f;

    //----------

    // Manages state of knockback
    private bool _isKnocked = false;

    GameManager gameManager;

    private void Awake()
    {
        // Assign component references
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
    public void KnockBack(Vector3 knockbackOrigin, float baseKnockbackForce)
    {
        if (_isKnockbackable && !_isKnocked)
        {
            // Inform game manager that item is destroyed

            gameManager.ItemDestroyed();
            Debug.Log("Knockback Applied");

            // Calculate the direction of the knockback
            Vector3 knockbackDirection = transform.position - knockbackOrigin;

            // Normalize the direction to eliminate the magnitude
            knockbackDirection.Normalize();

            // Set the state
            _isKnocked = true;

            // Add "Knockback"
            // I use ForceMode.VelocityChange becuase the designer will modify each enemies knockback force multiplier directly in this script's inspector.
            rb.AddForce(knockbackDirection * baseKnockbackForce * _knockbackForceMultplier, ForceMode.VelocityChange);
        }
    }

    public void Recover()
    {
        throw new System.NotImplementedException();
    }
}
