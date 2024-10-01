using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Transform parent;
    private Queue<T> poolQueue;

    public ObjectPool(T prefab, Transform parent, int initialSize)
    {
        this.prefab = prefab;
        this.parent = parent;
        poolQueue = new Queue<T>();

        for (int i = 0; i < initialSize; i++)
        {
            T newObj = GameObject.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            poolQueue.Enqueue(newObj);
        }
    }

    public T Get()
    {
        if (poolQueue.Count > 0)
        {
            T obj = poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T newObj = GameObject.Instantiate(prefab, parent);
            return newObj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
