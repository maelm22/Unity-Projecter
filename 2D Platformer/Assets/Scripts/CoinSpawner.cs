using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
   public GameObject[] spawnPoints;
    public GameObject coinPrefab;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.Range(0,2) == 1)
            Instantiate(coinPrefab, spawnPoints[i].transform.position, Quaternion.identity); 
        }
    }

    
}
