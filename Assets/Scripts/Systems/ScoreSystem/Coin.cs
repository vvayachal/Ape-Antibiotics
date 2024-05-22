using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ScoreManager.Instance.Score();
            Debug.Log("Score from Coin Collection. Score - "+ScoreManager.Instance.GetScore().ToString());
            Destroy(gameObject);
        }
    }
}
