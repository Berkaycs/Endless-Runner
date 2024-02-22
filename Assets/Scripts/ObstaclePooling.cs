using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooling : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObstacles;
        public GameObject obstaclePrefab;
        public int poolSize;
    }

    public Pool[] pools;
    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObstacles = new Queue<GameObject>();

            for (int i = 0;  i < pools[j].poolSize; i++)
            {
                GameObject obstacle = Instantiate(pools[j].obstaclePrefab);
                obstacle.SetActive(false);

                pools[j].pooledObstacles.Enqueue(obstacle);
            }
        }
    }

    // POOLA B�RDEN �OK ENGEL KOY 2L� YA DA �CL� VE B�R SPAWN NOKTASI OLSUN D�S�N 
}
