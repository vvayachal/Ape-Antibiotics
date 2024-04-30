using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabAndChuck : MonoBehaviour
{
    // State management variables for the class
    private bool _isCarryingObject = false;
    private Collider _currentHeldObject;

    //----------------------------

    [Header("Object To Hold Attributes")]

    [Tooltip("This will be a prefab with only mesh that will spawn in place of the original for the player to hold.")]
    [SerializeField] private GameObject _objectToReplacePrefab;

    //----------------------------

    [Header("Grabbing Attributes")]

    [Tooltip("The point where the player checks for any grabble objects.")]
    [SerializeField] private Transform _grabPoint;

    [Tooltip("Any layer that will contain grabbable objects.")]
    [SerializeField] private LayerMask _grabLayer;

    [Tooltip("The radius of the sphere collider used to check for grabbable objects.")]
    [SerializeField] private float _grabRadius;

    //---------------------------

    [Header("Holding Attributes")]

    [Tooltip("The point where currently held objects will go")]
    [SerializeField] private Transform _holdPoint;

    //----------------------------

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_isCarryingObject)
        {
            _currentHeldObject = ObjectToGrab();
            if (_currentHeldObject != null)
            {
                _isCarryingObject = true;
                HoldObject();
            }
        }
    }

    /// <summary>
    /// Method <c>ObjectToGrab</c> cycles through all colliders and determines which is closest to the player.
    /// </summary>
    /// <returns>Collider that is closest to player.</returns>
    private Collider ObjectToGrab()
    {
        Debug.Log("Activated");
        Collider[] allCurrentObjects = Physics.OverlapSphere(_grabPoint.position, _grabRadius, _grabLayer);
        Collider closestObject = null;
        //Declared with 100f to ensure proper logic
        float closestObjectDistance = 100f;

        foreach (Collider collider in allCurrentObjects)
        {
            if (Vector3.Distance(collider.transform.position, transform.position) < closestObjectDistance)
            {
                closestObject = collider;
                closestObjectDistance = Vector3.Distance(collider.transform.position, transform.position);
            }
        }

        return closestObject;
    }

    private void HoldObject()
    {
        //TODO: Instead of using that object itself to hold, instantiate the model of the object and disable the original.

        // Get the necessary data from the object to create its clone
        Mesh currentObjectMesh = _currentHeldObject.GetComponentInChildren<MeshFilter>().mesh;
        Material currentObjectMaterial = _currentHeldObject.GetComponentInChildren<MeshRenderer>().material;

        // Clone the original object
        GameObject objectToHoldClone = Instantiate(_objectToReplacePrefab, _holdPoint.position, Quaternion.identity, _holdPoint);
        objectToHoldClone.GetComponent<MeshFilter>().mesh = currentObjectMesh;
        objectToHoldClone.GetComponent<MeshRenderer>().material = currentObjectMaterial;

        // Disable the original
        _currentHeldObject.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        // Draw a green sphere for debugging [Tegomlee]
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_grabPoint.position, _grabRadius);
    }
}
