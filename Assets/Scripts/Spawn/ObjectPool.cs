using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly List<GameObject> pool;
    private readonly GameObject prefab;
    private readonly Transform parentTransform;

    public ObjectPool(GameObject prefab, int initialSize, Transform parentTransform = null)
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;
        pool = new List<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parentTransform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject Get()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = GameObject.Instantiate(prefab, parentTransform);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
