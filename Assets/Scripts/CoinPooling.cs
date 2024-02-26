using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPooling : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledCoins;
        public GameObject coinPrefab;
        public int PoolSize;
    }

    public Pool[] pools;

    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledCoins = new Queue<GameObject>();

            for (int i = 0; i < pools[j].PoolSize; i++)
            {
                GameObject coin = Instantiate(pools[j].coinPrefab);
                coin.SetActive(false);

                pools[j].pooledCoins.Enqueue(coin);
            }
        }
    }
}
