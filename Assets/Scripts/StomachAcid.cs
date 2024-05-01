using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachAcid : MonoBehaviour
{
    [SerializeField] private float damage;
    
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
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Shield>().TakeDamage(damage);
            }
        }
}
