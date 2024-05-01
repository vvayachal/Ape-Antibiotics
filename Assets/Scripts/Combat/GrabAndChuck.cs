using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrabAndChuck : MonoBehaviour
{
    // State management variables for the class
    private bool _isCarryingObject = false;
    private GameObject _currentHeldObject = null;
    private GameObject _currentHeldCloneObject = null;

    //----------------------------

    [Header("Neccesary Prefabs")]

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

    [Header("Chucking Attributes")]
    [SerializeField] private float _chuckForce;

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

        if (Input.GetMouseButtonUp(1) && _isCarryingObject)
        {
            ChuckObject();
        }
    }

    /// <summary>
    /// Method <c>ObjectToGrab</c> cycles through all colliders and determines which is closest to the player.
    /// </summary>
    /// <returns><c>GameObject</c> that is closest to player.</returns>
    private GameObject ObjectToGrab()
    {
        // This can be modified to grab the object closest to the grab point if the design deems it so. [Tegomlee]

        Debug.Log("Activated");
        Collider[] allCurrentObjects = Physics.OverlapSphere(_grabPoint.position, _grabRadius, _grabLayer);
        GameObject closestObject = null;
        //Declared with 100f to ensure proper logic
        float closestObjectDistance = 100f;

        foreach (Collider collider in allCurrentObjects)
        {
            if (Vector3.Distance(collider.transform.position, transform.position) < closestObjectDistance)
            {
                closestObject = collider.gameObject;
                closestObjectDistance = Vector3.Distance(collider.transform.position, transform.position);
            }
        }

        return closestObject;
    }

    /// <summary>
    /// Method <c>HoldObject</c> creates a clone of the object that is parented to the hold point to simulate the player carrying it.
    /// </summary>
    private void HoldObject()
    {
        //TODO: Instead of using that object itself to hold, instantiate the model of the object and disable the original. [Complete]

        // Get the necessary data from the object to create its clone
        Mesh currentObjectMesh = _currentHeldObject.GetComponentInChildren<MeshFilter>().mesh;
        Material currentObjectMaterial = _currentHeldObject.GetComponentInChildren<MeshRenderer>().material;

        // Clone the original object
        _currentHeldCloneObject = Instantiate(_objectToReplacePrefab, _holdPoint.position, Quaternion.identity, _holdPoint);
        _currentHeldCloneObject.GetComponent<MeshFilter>().mesh = currentObjectMesh;
        _currentHeldCloneObject.GetComponent<MeshRenderer>().material = currentObjectMaterial;

        // Disable the original
        _currentHeldObject.gameObject.SetActive(false);
    }

    private void ChuckObject()
    {
        //TODO: Restore the original object and destroy the clone. Use the player's knockback script to chuck the object. [Complete]

        // Get the camera's rotation
        Quaternion currentRotation = GetComponentInChildren<Camera>().transform.rotation;

        // Delete the clone
        Destroy(_currentHeldCloneObject);

        // Restore the original object and set its transform
        _currentHeldObject.transform.position = _holdPoint.position;
        _currentHeldObject.transform.rotation = currentRotation;
        _currentHeldObject.SetActive(true);

        // Apply the "Knockback"
        Knockback knockback = GetComponent<Knockback>();
        StartCoroutine(knockback.ApplyKnockBack(_currentHeldObject.GetComponent<CapsuleCollider>(), _holdPoint.position, _chuckForce));

        // Reset the class state
        _currentHeldObject = null;
        _currentHeldCloneObject = null;
        _isCarryingObject = false;
    }

    private void OnDrawGizmos()
    {
        // Draw a green sphere for debugging [Tegomlee]
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_grabPoint.position, _grabRadius);
    }
}
