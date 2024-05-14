using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    private static Knockback instance;
    public static Knockback Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Knockback>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public void PerformKnockback(Collider affectedCollider, Vector3 originOfForcePosition, float knockbackForce, bool isModified = false)
    {
        // Check if the collider has an attached rigidbody
        if (affectedCollider.TryGetComponent(out Rigidbody affectedRigidBody))
        {
            // Get the affected collider's position
            Vector3 affectedColliderPosition = affectedRigidBody.position;

            // Get the direction of the force
            Vector3 directionOfForce = CalculatedDirection(affectedColliderPosition, originOfForcePosition);

            // Modify the vector if necessary
            if (isModified)
                directionOfForce += Vector3.up;

            // Disable conflicting components
            SetComponentState(affectedCollider, false);

            // Apply the force to the object
            affectedRigidBody.AddForce(directionOfForce * knockbackForce, ForceMode.VelocityChange);
        }
        else
        {
            // Print a log
            Debug.LogWarning("The collider has no attached Rigidbody Component.");
        }
    }

    public void SetComponentState(Collider affectedCollider, bool activeState)
    {
        if (affectedCollider.TryGetComponent(out Animator affectedAnimator))
        {
            affectedAnimator.enabled = activeState;
        }

        if (affectedCollider.TryGetComponent(out NavMeshAgent affectedNavMeshAgent))
        {
            affectedNavMeshAgent.enabled = activeState;
        }
    }

    private Vector3 CalculatedDirection(Vector3 targetPosition, Vector3 originPosition)
    {
        // Calculate the direction of the knockback and normalize it
        Vector3 direction = targetPosition - originPosition;
        direction.Normalize();

        return direction;
    }
}
