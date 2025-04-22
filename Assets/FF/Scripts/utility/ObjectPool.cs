using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Transform parent;
    private Queue<T> pool = new();

    public ObjectPool(T prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public T Get()
    {
        if (pool.Count > 0)
            return Activate(pool.Dequeue());

        var newItem = GameObject.Instantiate(prefab, parent);
        return Activate(newItem);
    }

    public void Release(T item)
    {
        item.gameObject.SetActive(false);
        pool.Enqueue(item);
    }

    private T Activate(T item)
    {
        item.gameObject.SetActive(true);
        return item;
    }
}
