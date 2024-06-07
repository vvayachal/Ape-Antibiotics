using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]private float speed;
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
