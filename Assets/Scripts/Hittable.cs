using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public MyIntEvent onhit; 

    public void Hit(int damage, int knockback)
    {
        onhit?.Invoke(damage,knockback); // if theres an event, calls the function and passes in damage and knockback
    }
}

[System.Serializable]
public class MyIntEvent : UnityEvent<int, int> // custom unity event to pass in two ints
{
}
