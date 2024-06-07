using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : MonoBehaviour
{
    public LayerMask enemyLayers;

    public float descendRate = 50f;
    public float aoeDamage;
    //public float directDamage;
    //public float directRange;
    public float aoeRange;

    //float minCooldown = 0f;
    public float maxCooldown = 5f;
    public float curCooldown;
    public float cooldown;

    private Knockback knockb;

    //public Transform directHitbox;
    public Transform aoeHitbox;

    List<Collider> hitTargets;

    PlayerMovement pm;
    Rigidbody rb;

    //make the ground pound similar to ultrakill

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        knockb = GetComponent<Knockback>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if((pm.isJumping || !this.GetComponent<DoubleJump>().isGrounded) && cooldown <= 0) 
            {
                //adds in additional damage based on height, very basic will be redone to track height at jump
                float yAtGroundPound = this.transform.position.y;
                float curAoeDamage = aoeDamage;
                aoeDamage += yAtGroundPound;
                PerformGroundPound();
                Debug.Log("did " + aoeDamage + " damage.");
                aoeDamage = curAoeDamage;
            }
        }

        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        } else
        {
            cooldown = 0;
            Debug.Log("Ground pound is ready to be used!");
        }
}

    private void FixedUpdate()
    {

    }

    void PerformGroundPound()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * -(descendRate), ForceMode.Impulse);

        //detectDirectHit();
        detectAOEHits();
    }

    /*void detectDirectHit()
    {
        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(directHitbox.position, directRange, affectedLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this
            Debug.Log($"{this.name} has ground pounded {enemy.name}");
            if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
            {
                enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(directDamage);
            }

            // Knock them back
            StartCoroutine(knockb.ApplyKnockBack(enemy, directHitbox.position, directDamage));
        }

        aoeHitbox.gameObject.SetActive(true);
    }*/

    void detectAOEHits()
    {
        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(aoeHitbox.position, aoeRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // pretty inefficient because we're using
            // getcomponent twice but idk how to really optimize this
            Debug.Log($"{this.name} has knocked {enemy.name} away!");
            if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
            {
                enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(aoeDamage);
            }

            // Knock them back
            //StartCoroutine(knockb.ApplyKnockBack(enemy, aoeHitbox.position, aoeDamage));
        }

        cooldown = maxCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        /*if (directHitbox == null) return;

        if (directHitbox.parent.gameObject.activeSelf)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(directHitbox.position, directRange);
        }*/

        if (aoeHitbox == null) return;

        if (aoeHitbox.parent.gameObject.activeSelf)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(aoeHitbox.position, aoeRange);
        }
    }
}


