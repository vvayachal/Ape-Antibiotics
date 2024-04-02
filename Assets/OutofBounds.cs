using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    public GameObject spawn;

    public void OnTriggerEnter(Collider other)
    {
        print("other");
            other.transform.position = spawn.transform.position;
            
    }
}
