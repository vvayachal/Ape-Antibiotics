using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashCooldownAnim : MonoBehaviour
{
    public Punch punch;

    private void Start()
    {
        
    }

    public void CanPunch()
    {
        punch.canPunch = true;
    }

    public void NoPunch()
    {
        punch.canPunch = false;
    }
}
