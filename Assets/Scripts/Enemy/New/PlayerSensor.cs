using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors
{

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
            if (other.tag == "Player")
            {
                OnPlayerEnter?.Invoke(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
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
}