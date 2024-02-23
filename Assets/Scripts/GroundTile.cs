using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public List<GameObject> activeObstacles = new List<GameObject>();

    private GroundPooling groundPooling;
    private ObstaclePooling obstaclePooling;
    private CoinPooling coinPooling;

    private bool canSpawnObstacle = false;
    private bool canSpawnCoin = false;

    private void Awake()
    {
        groundPooling = GameObject.FindObjectOfType<GroundPooling>();
        obstaclePooling = GameObject.FindObjectOfType<ObstaclePooling>();
        coinPooling = GameObject.FindObjectOfType<CoinPooling>();
    }

    private void Start()
    {
        if (groundPooling.GetSpawnedTilesCount() > 2)
        {
            canSpawnObstacle = true;
            canSpawnCoin = true;
            SpawnObstacle();
            SpawnCoin();
        }
    }

    private void OnEnable()
    {
        if (groundPooling.GetSpawnedTilesCount() > 3)
        {
            canSpawnObstacle = true;
            canSpawnCoin = true;
            SpawnObstacle();
            SpawnCoin();
        }
    }

    public void SpawnObstacle()
    {
        if (!canSpawnObstacle) return;

        int obstacleSpawnIndex = Random.Range(2,4);
        int obstacleTypeGenerator;
        Transform spawnPoint;
        GameObject obstacle;

        if (activeObstacles.Count > 0)
        {
            Debug.Log("An obstacle is already active. Cannot spawn another one.");
            return;
        }

        switch (obstacleSpawnIndex)
        {
            default:
                return;
            case 2:
                obstacleTypeGenerator = Random.Range(14, 19);
                spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
                obstacle = GetObstacleFromPool(spawnPoint, obstacleTypeGenerator);
                activeObstacles.Add(obstacle);
                break;
            case 3:
                obstacleTypeGenerator = Random.Range(0, 19);
                spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
                obstacle = GetObstacleFromPool(spawnPoint, obstacleTypeGenerator);
                activeObstacles.Add(obstacle);
                break;
        }
    }

    public void SpawnCoin()
    {
        if (!canSpawnCoin) return;

        int spawnCoinPointNumber = Random.Range(0, 3); // choosing one or two spawn point in the one tile
        int coinTypeGenerator = Random.Range(0, 2); // choosing to spawn silver or gold coin from the pool
        int coinSpawnIndex;
        int spawnCoinNumber;

        switch (spawnCoinPointNumber)
        {
            default:
                return;
            case 0: // there is 0 spawn point
                return;
            case 1: // there is 1 spawn point
                coinSpawnIndex = Random.Range(4, 7);
                Transform spawnPoint = transform.GetChild(coinSpawnIndex).transform;
                spawnCoinNumber = Random.Range(3,6); // how many coins will spawn?
                for (int i = 0; i < spawnCoinNumber; i++)
                {
                    GameObject coin = GetCoinFromPool(spawnPoint, coinTypeGenerator);
                    activeObstacles.Add(coin);
                    spawnPoint.position += new Vector3(0, 0, 3);
                }
                break;
            case 2: // there is 2 spawn point
                int previousIndex = -1;
                for (int j = 0; j < spawnCoinPointNumber; j++)
                {
                    coinSpawnIndex = Random.Range(4, 7);
                    spawnPoint = transform.GetChild(coinSpawnIndex).transform;

                    if (previousIndex != coinSpawnIndex) // if the second spawn point index is not same with the first spawn point index, then spawn the coin to the second spawn point
                    {
                        spawnCoinNumber = Random.Range(3, 6); ;
                        for (int i = 0; i < spawnCoinNumber; i++)
                        {
                            GameObject coin = GetCoinFromPool(spawnPoint, coinTypeGenerator);
                            activeObstacles.Add(coin);
                            spawnPoint.position += new Vector3(0, 0, 3);
                        }
                    }
                    previousIndex = coinSpawnIndex;
                }
                break;
        }
    }

    public GameObject GetObstacleFromPool(Transform spawnPosition, int objectType)
    {
        if (objectType >= obstaclePooling.pools.Length) return null;

        GameObject obstacle = obstaclePooling.pools[objectType].pooledObstacles.Dequeue();
        obstacle.transform.position = spawnPosition.transform.position;
        obstacle.SetActive(true);
        obstaclePooling.pools[objectType].pooledObstacles.Enqueue(obstacle);
        return obstacle;
    }

    public GameObject GetCoinFromPool(Transform spawnPosition, int objectType)
    {
        if (objectType >= coinPooling.pools.Length) return null;

        GameObject coin = coinPooling.pools[objectType].pooledCoins.Dequeue();
        coin.transform.position = spawnPosition.transform.position;
        coin.SetActive(true);
        coinPooling.pools[objectType].pooledCoins.Enqueue(coin);
        return coin;
    }

    private void DeactivateObstacles()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            obstacle.SetActive(false);
        }
        activeObstacles.Clear();
    }

    private void OnTriggerExit(Collider other)
    {
        DeactivateObstacles();
        groundPooling.GetGroundFromPool();
        gameObject.SetActive(false);
    }
}
