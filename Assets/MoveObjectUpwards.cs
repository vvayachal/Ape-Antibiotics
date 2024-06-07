using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveObjectUpwards : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private Vector3 initialPosition;

    [SerializeField]private bool shouldMove;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime); 
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.transform.position = initialPosition;
        }
    }
}
