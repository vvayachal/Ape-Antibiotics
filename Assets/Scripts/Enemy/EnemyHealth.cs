using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private bool scoreOnDestroy;
    
    [Tooltip("Number of coins to drop on destroying the object")]
    [SerializeField] private int coinsToDrop;
    
    [SerializeField] float hitPoints = 100f;
    [SerializeField] float timeBeforeDestroy = 3f;
    Animator animator;
    public Image healthBar;
    public float timeBeforeReposition = 5f;
    
    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(healthBar)
        {
            healthBar.fillAmount = hitPoints / 100;    
        }
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

        if (scoreOnDestroy)
        {
            ScoreManager.Instance.Score();
            Debug.Log("Score from Player Death. Score - "+ScoreManager.Instance.GetScore().ToString());    
        }
        else
        {
            ScoreManager.Instance.DropCoins(coinsToDrop, gameObject.transform.position);
            
            //Destroy(gameObject);
        }

        NavMeshAgent navMeshAgent;
        if (TryGetComponent<NavMeshAgent>(out navMeshAgent))
        {
            navMeshAgent.enabled = false;    
        }

        if (animator)
        {
            animator.SetTrigger("die");    
        }
        
        //Destroy(gameObject, timeBeforeDestroy);
    }
}
