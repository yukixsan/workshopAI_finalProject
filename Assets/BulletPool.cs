using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public List<Bullet> bulletPrefabs; // Prefabs for bullets
    public int poolSize = 10; // Number of bullets per type to pre-instantiate

    private Dictionary<int, Queue<Bullet>> poolDictionary;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        poolDictionary = new Dictionary<int, Queue<Bullet>>();

        // Initialize the pool for each prefab
        for (int i = 0; i < bulletPrefabs.Count; i++)
        {
            Queue<Bullet> objectPool = new Queue<Bullet>();

            for (int j = 0; j < poolSize; j++)
            {
                Bullet obj = Instantiate(bulletPrefabs[i], transform);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(i, objectPool);
        }
    }

    public Bullet SpawnFromPool(int index, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(index)) return null;

        Bullet objectToSpawn = poolDictionary[index].Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = position;

        poolDictionary[index].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
