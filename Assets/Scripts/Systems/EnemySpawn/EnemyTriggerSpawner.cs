using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyTriggerSpawner : MonoBehaviour
{
    /// <summary>
    /// Class <c>SpawnPoint</c> acts as a constructor grabbing all the neccesary information to properly spawn enemies.
    /// </summary>
    [Serializable]
    private class SpawnPoint
    {
        [Tooltip("This is the enemy that will spawn.")]
        [SerializeField] private GameObject _enemyToSpawn;
        public GameObject EnemyToSpawn {  get { return _enemyToSpawn; } }

        [Tooltip("These are the points where the enemy will spawn at.")]
        [SerializeField] private Transform[] _enemySpawnPoints;
        public Transform[] EnemySpawnPoints { get {  return _enemySpawnPoints; } }

        [Tooltip("This is the amount of time needed to pass to spawn the enemy.")]
        [SerializeField] private float _timeNeededToSpawnEnemy;

        // This handles the state of each spawnpoint to prevent retriggering.
        private bool _hasTriggered = false;

        /// <summary>
        /// Method <c>CanTrigger</c> checks wether the spawnpoint has triggered, and if enough time has passed.
        /// </summary>
        /// <returns>Whether the enemies in that spawn point can spawn.</returns>
        public bool CanTrigger()
        {
            if (!_hasTriggered && _timeNeededToSpawnEnemy <= Time.timeSinceLevelLoad)
            {
                _hasTriggered = true;
                return true;
            }
            else
                return false;
        }
    }

    //----------------

    [Tooltip("This is the list of enemies that will spawn with this trigger.")]
    [SerializeField] private SpawnPoint[] _spawnPoints;

    [Tooltip("The layers that can interact with the spawn trigger (Usually the player).")]
    [SerializeField] private LayerMask _layerThatCanTrigger;

    //----------------

    // Used to track the triggering objects position to make the enemies look at that object when spawning
    private Vector3 _triggeringObjectPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggeringObjectPosition = other.transform.position;
            SpawnEnemies();
            Debug.Log("Enemy Spawned");
        }
    }

    /// <summary>
    /// Method <c>SpawnEnemy</c> loops through each spawn point attached to this trigger and spawns the associated enemies if the conditions are met.
    /// </summary>
    private void SpawnEnemies()
    {
        Debug.Log("Enemy Spawned");
        if (_spawnPoints == null) 
        {
            Debug.LogWarning("This trigger is not properly set up");
            return;
        }

        foreach (SpawnPoint spawnPoint in _spawnPoints)
        {
            if (spawnPoint.CanTrigger())
            {
                for (int i = 0; i < spawnPoint.EnemySpawnPoints.Length; i++)
                {
                    // Get the rotation for the enemy to spawn looking at the player
                    Vector3 lookDirection = _triggeringObjectPosition - spawnPoint.EnemySpawnPoints[i].position;
                    Quaternion enemyRotation = Quaternion.LookRotation(lookDirection);

                    // Spawn the enemy at the target position and rotation
                    Instantiate(spawnPoint.EnemyToSpawn, spawnPoint.EnemySpawnPoints[i].position, enemyRotation);
                }
            }
        }
    }

    //This is to make it easier for designers to place the Spawn Trigger [Tegomlee]
    private void OnDrawGizmos()
    {
        Color colorOfTrigger = Color.red;
        colorOfTrigger.a = 0.5f;

        Gizmos.color = colorOfTrigger;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
