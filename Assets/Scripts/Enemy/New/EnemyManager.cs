using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    float hitPoints;
    public EnemyObject enemyObj;
    [SerializeField]
    float timeBeforeDestroy = 3f;
    [SerializeField]
    public float timeBeforeReposition = 5f;

    Animator anim;

    public Image healthBar;

    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        hitPoints = enemyObj.enemyHp;
    }

    void Update()
    {
        healthBar.fillAmount = hitPoints / 100;
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        //Debug.Log($"{this.name} health: {hitPoints}");

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<NavMeshAgent>().enabled = false;
        anim.SetTrigger("die");
        Destroy(gameObject, timeBeforeDestroy);
    }
}
