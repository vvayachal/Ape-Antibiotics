using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] EnemyHealth enemy;

    [Header("Keybinds")]
    [SerializeField] KeyCode inflictDamageKey = KeyCode.R;

    void Update()
    {
        AttackEnemy();
    }

    private void AttackEnemy()
    {
        if (Input.GetKeyDown(inflictDamageKey))
        {
            enemy.TakeDamage(10f);
        }
    }
}
