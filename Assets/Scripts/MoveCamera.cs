using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Transform cameraPosition;

    void Start()
    {
        cameraPosition = FindObjectOfType<CameraPosition>().transform;
    }

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
