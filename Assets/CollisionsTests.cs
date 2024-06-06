using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsTests : MonoBehaviour
{
    public GameObject RespawnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Hazard")
        {
            this.gameObject.transform.position = RespawnObject.transform.position;
        }
    }
}
