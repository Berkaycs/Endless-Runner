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
        public int PoolSize;
    }

    public Pool[] pools;
    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObstacles = new Queue<GameObject>();

            for (int i = 0;  i < pools[j].PoolSize; i++)
            {
                GameObject obstacle = Instantiate(pools[j].obstaclePrefab);
                obstacle.SetActive(false);

                pools[j].pooledObstacles.Enqueue(obstacle);
            }
        }
    }
}
