using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerSensor : MonoBehaviour
{
    public Color gizmoColor;

    public float radius;

    public delegate void PlayerEnterEvent(Transform player);

    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;

    public event PlayerExitEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement player))
        {
            OnPlayerEnter?.Invoke(player.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement player))
        {
            OnPlayerExit?.Invoke(other.transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
