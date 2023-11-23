using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<string, LinkedList<GameObject>> _pool = new Dictionary<string, LinkedList<GameObject>>();

    private static Transform _parentForPoolObjects;

    public static void PutObject(GameObject gameObject)
    {
        if (_pool.ContainsKey(gameObject.name))
        {
            _pool[gameObject.name].AddLast(gameObject);
        }
        else
        {
            _pool[gameObject.name] = new LinkedList<GameObject>();
            _pool[gameObject.name].AddLast(gameObject);
        }
        gameObject.transform.SetParent(_parentForPoolObjects);
        gameObject.SetActive(false);
    }

    public static GameObject GetObject(GameObject gameObject)
    {
        var key = gameObject.name;
        if (_pool.ContainsKey(key))
        {
            if (_pool[key].Count > 0)
            {
                var result = _pool[key].Last.Value;
                _pool[key].RemoveLast();
                return result;
            }
            else
            {
                var result = GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
                result.name = gameObject.name;
                return result;
            }
        }
        else
        {
            _pool[gameObject.name] = new LinkedList<GameObject>();
            var result = GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
            result.name = gameObject.name;
            return result;
        }
    }

    public static void SetParentForPoolObjects(Transform parent)
    {
        _parentForPoolObjects = parent;
    }

    public static void InitializePool(GameObject prefab, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var poolObject = GameObject.Instantiate(prefab, _parentForPoolObjects);
            poolObject.name = prefab.name;
            PutObject(poolObject);
        }
    }
}