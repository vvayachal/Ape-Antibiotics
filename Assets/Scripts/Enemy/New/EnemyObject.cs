using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/Create Enemy", order = 1)]
public class EnemyObject : ScriptableObject
{
    /**
     * the point of this script is to not make a hundred thousand 
     * scripts of different enemies, you give the enemies their 
     * stats here and use them in the enemy manager
     */
    public string enemyName;

    public int enemyHp;
}
