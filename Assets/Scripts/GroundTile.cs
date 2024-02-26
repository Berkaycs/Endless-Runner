using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private List<GameObject> _activeObstacles = new List<GameObject>();

    private GroundPooling _groundPooling;
    private ObstaclePooling _obstaclePooling;
    private CoinPooling _coinPooling;

    private bool _canSpawnObstacle = false;
    private bool _canSpawnCoin = false;

    private void Awake()
    {
        _groundPooling = GameObject.FindObjectOfType<GroundPooling>();
        _obstaclePooling = GameObject.FindObjectOfType<ObstaclePooling>();
        _coinPooling = GameObject.FindObjectOfType<CoinPooling>();
    }

    private void Start()
    {
        if (_groundPooling.GetSpawnedTilesCount() > 2)
        {
            _canSpawnObstacle = true;
            _canSpawnCoin = true;
            SpawnObstacle();
            SpawnCoin();
        }
    }

    private void OnEnable()
    {
        if (_groundPooling.GetSpawnedTilesCount() > 2)
        {
            _canSpawnObstacle = true;
            _canSpawnCoin = true;
            SpawnObstacle();
            SpawnCoin();
        }
    }

    IEnumerator HideGroundAfterSeconds()
    {
        yield return new WaitForSeconds(7f);
        DeactivateObstacles();
        gameObject.SetActive(false);
    }

    public void SpawnObstacle()
    {
        if (!_canSpawnObstacle) return;

        int obstacleSpawnIndex = Random.Range(2,4);
        int obstacleTypeGenerator;
        Transform spawnPoint;
        GameObject obstacle;

        if (_activeObstacles.Count > 0)
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
                _activeObstacles.Add(obstacle);
                break;
            case 3:
                obstacleTypeGenerator = Random.Range(0, 19);
                spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
                obstacle = GetObstacleFromPool(spawnPoint, obstacleTypeGenerator);
                _activeObstacles.Add(obstacle);
                break;
        }
    }

    public void SpawnCoin()
    {
        if (!_canSpawnCoin) return;

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
                    _activeObstacles.Add(coin);
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
                            _activeObstacles.Add(coin);
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
        if (objectType >= _obstaclePooling.pools.Length) return null;

        GameObject obstacle = _obstaclePooling.pools[objectType].pooledObstacles.Dequeue();
        obstacle.transform.position = spawnPosition.transform.position;
        obstacle.SetActive(true);
        _obstaclePooling.pools[objectType].pooledObstacles.Enqueue(obstacle);
        return obstacle;
    }

    public GameObject GetCoinFromPool(Transform spawnPosition, int objectType)
    {
        if (objectType >= _coinPooling.pools.Length) return null;

        GameObject coin = _coinPooling.pools[objectType].pooledCoins.Dequeue();
        coin.transform.position = spawnPosition.transform.position;
        coin.SetActive(true);
        _coinPooling.pools[objectType].pooledCoins.Enqueue(coin);
        return coin;
    }

    private void DeactivateObstacles()
    {
        foreach (GameObject obstacle in _activeObstacles)
        {
            obstacle.SetActive(false);
        }
        _activeObstacles.Clear();
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(HideGroundAfterSeconds());
        _groundPooling.GetGroundFromPool();
    }
}
