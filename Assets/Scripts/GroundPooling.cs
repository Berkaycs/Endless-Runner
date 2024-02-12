using System.Collections.Generic;
using UnityEngine;

public class GroundPooling : MonoBehaviour
{
    private Queue<GameObject> pooledGrounds; // creating a queue
    [SerializeField] private GameObject groundPrefab; // assigning a prefab
    [SerializeField] private int poolSize = 10; // define the total size


    public int startSize = 2;

    private Vector3 nextSpawnPoint;

    // Initializing the objects
    private void Awake()
    {
        pooledGrounds = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject ground = Instantiate(groundPrefab);
            ground.SetActive(false);
            pooledGrounds.Enqueue(ground);
        }
    }

    private void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            GameObject ground = pooledGrounds.Dequeue();
            ground.transform.position = nextSpawnPoint;
            nextSpawnPoint = ground.transform.GetChild(1).transform.position;
            ground.SetActive(true);
            pooledGrounds.Enqueue(ground);
        }
    }

    // Returning the objects when it is needed
    public GameObject GetPooledObject()
    {
        GameObject ground = pooledGrounds.Dequeue();
        ground.transform.position = nextSpawnPoint; // Update ground position
        nextSpawnPoint = ground.transform.GetChild(1).transform.position; // Update nextSpawnPoint
        ground.SetActive(true);
        pooledGrounds.Enqueue(ground);
        return ground;
    }
}
