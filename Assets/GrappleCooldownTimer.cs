using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleCooldownTimer : MonoBehaviour
{
    public Text timer;
    public Grappling grapplingScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer.text = Mathf.RoundToInt(grapplingScript.grapplingCdTimeLeft).ToString();
    }
}
