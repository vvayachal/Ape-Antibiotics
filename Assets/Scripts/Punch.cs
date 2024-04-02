using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Animator anim;
    public Collider punch_collider;
    float lastfired;
    float FireRate = 20f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPunch();
        }
    }   

    public IEnumerator OpenCloseCollider()
    {
        punch_collider.enabled = true;
        yield return new WaitForSeconds(0.03f);
        punch_collider.enabled = false;
    }

    public void StartPunch()
    {
        if (Time.time - lastfired > 1 / FireRate)
        {
            lastfired = Time.time;
            anim.SetTrigger("Punch");
        }
    }

    public void AnimEvent()
    {
        StartCoroutine(OpenCloseCollider());
    }
}
