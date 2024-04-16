using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] Image image;
    Punch punchScript;

    bool isCoolingDown;
    float punchTime = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        punchScript = FindObjectOfType<Punch>();
    }

    private void Update()
    {
        if (isCoolingDown)
        {
            punchTime += Time.deltaTime;
        }

        image.fillAmount = punchTime / punchScript.FireRate;
    }

    public void ActivateCooldown()
    {
        Debug.Log("We punchin");

        punchTime = 0;
        isCoolingDown = true;
        image.fillAmount = 0;
    }
}
