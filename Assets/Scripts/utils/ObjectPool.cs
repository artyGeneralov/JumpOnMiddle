
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    GameObject pooledObject;
    int initialPoolSize;
    List<GameObject> pool;
    public ObjectPool(GameObject pooledObject, int initialPoolSize)
    {
        pool = new List<GameObject>();
        this.initialPoolSize = initialPoolSize;
        this.pooledObject = pooledObject;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(pooledObject);
            obj.SetActive(false);
            pool.Add(obj);
        }
        
    }

    public GameObject GetPooledObject()
    {
        
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = GameObject.Instantiate(pooledObject);
        obj.SetActive(true);
        pool.Add(obj);
        return obj;
    }

    public void DeactivateAndPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public int getPoolSize()
    {
        return pool.Count;
    }
}
