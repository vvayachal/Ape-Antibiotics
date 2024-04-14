using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform targetCamera;

    void Update()
    {
        transform.LookAt(targetCamera);
    }
}
