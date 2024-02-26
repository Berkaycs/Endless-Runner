using System.Collections.Generic;
using UnityEngine;

public class GroundPooling : MonoBehaviour
{
    private Queue<GameObject> _pooledGrounds; // creating a queue
    [SerializeField] private GameObject _groundPrefab; // assigning a prefab
    [SerializeField] private int _poolSize = 30; // define the total size

    private List<GameObject> _spawnedGroundTiles = new List<GameObject>();

    private int _startSize = 7;

    private Vector3 nextSpawnPoint;

    // Initializing the objects
    private void Awake()
    {
        _pooledGrounds = new Queue<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            if (_groundPrefab != null)
            {
                GameObject ground = Instantiate(_groundPrefab);
                ground.SetActive(false);
                _pooledGrounds.Enqueue(ground);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < _startSize; i++)
        {
            GameObject ground = _pooledGrounds.Dequeue();
            ground.transform.position = nextSpawnPoint;
            nextSpawnPoint = ground.transform.GetChild(1).transform.position;
            ground.SetActive(true);
            _pooledGrounds.Enqueue(ground);
        }
    }

    public GameObject GetGroundFromPool()
    {
        GameObject ground = _pooledGrounds.Dequeue();
        ground.transform.position = nextSpawnPoint;
        nextSpawnPoint = ground.transform.GetChild(1).transform.position;
        ground.SetActive(true);
        _pooledGrounds.Enqueue(ground);
        _spawnedGroundTiles.Add(ground);
        return ground;
    }

    public int GetSpawnedTilesCount()
    {
        return _spawnedGroundTiles.Count;
    }
}
