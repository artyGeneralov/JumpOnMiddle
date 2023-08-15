
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using System;

public class NotchPool
{
    public enum PrefabType
    {
        LargeNotch,
        SmallNotch
    }

    private Dictionary<PrefabType, ObjectPool<GameObject>> poolDictionary = new Dictionary<PrefabType, ObjectPool<GameObject>>();
    private Dictionary<PrefabType, List<GameObject>> activeNotches = new Dictionary<PrefabType, List<GameObject>>();

    public NotchPool(GameObject largeNotchPrefab, GameObject smallNotchPrefab, int initialCount)
    {
        poolDictionary[PrefabType.LargeNotch] = new ObjectPool<GameObject>(() => GameObject.Instantiate(largeNotchPrefab),
                                                                            actionOnGet: (obj) => obj.SetActive(true),
                                                                            actionOnRelease: (obj) => obj.SetActive(false),
                                                                            defaultCapacity: initialCount);
        poolDictionary[PrefabType.SmallNotch] = new ObjectPool<GameObject>(() => GameObject.Instantiate(smallNotchPrefab),
                                                                            actionOnGet: (obj) => obj.SetActive(true),
                                                                            actionOnRelease: (obj) => obj.SetActive(false),
                                                                            defaultCapacity: initialCount);


    }

    public GameObject Get(PrefabType type)
    {

        GameObject obj = poolDictionary[type].Get();
        if (!activeNotches.ContainsKey(type))
        {
            activeNotches[type] = new List<GameObject>();
        }
        activeNotches[type].Add(obj);
        return obj;
    }

    public void Release(GameObject obj)
    {
        PrefabType type = 0;
        if (obj.tag == "LargeNotch")
            type = PrefabType.LargeNotch;
        else if (obj.tag == "SmallNotch")
            type = PrefabType.SmallNotch;
        if(activeNotches.ContainsKey(type) && activeNotches[type].Contains(obj))
        {
            activeNotches[type].Remove(obj);
            poolDictionary[type].Release(obj);
        }
    }

    public void ForEachActiveNotch(Action<GameObject> action)
    {
        foreach(PrefabType type in activeNotches.Keys)
        {
            foreach(GameObject obj in activeNotches[type])
            {
                action(obj);
            }
        }
    }









    /*public ObjectPool(GameObject pooledObject, int initialPoolSize)
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
    }*/
}
