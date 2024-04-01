using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    //when turned on and something is colliding it checks if other has hittable, if yes call hit
    public int damage, knockback;
    public void OnTriggerEnter(Collider other)
    {
        return;
        if (other.gameObject.GetComponent<Hittable>())
        {
            other.gameObject.GetComponent<Hittable>().Hit(damage,knockback);
        }
    }
}
