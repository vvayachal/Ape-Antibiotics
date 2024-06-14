using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsTests : MonoBehaviour
{
    public GameObject RespawnObject1;

    public GameObject RespawnObject2;

    public GameObject RespawnObject3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hazard1")
        {
            this.gameObject.transform.position = RespawnObject1.transform.position;
        }

         if(other.gameObject.tag == "Hazard2")
        {
            this.gameObject.transform.position = RespawnObject2.transform.position;
        }

         if(other.gameObject.tag == "Hazard3")
        {
            this.gameObject.transform.position = RespawnObject3.transform.position;
        }
    }
}
