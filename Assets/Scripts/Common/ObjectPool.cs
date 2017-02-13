using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : Singleton<ObjectPool>
{
    class Pool
    {
        public Pool(Object _prefab)
        {
            prefab = _prefab;
        }
        public Object prefab;
        public List<GameObject> recycled = new List<GameObject>();
    }

    Dictionary<string, Pool> pool = new Dictionary<string, Pool>();
    int _activeCount = 0;
    int _reservedCount = 0;


    public static int ActiveCount { get { return Instance._activeCount; } }
    public static int ReservedCount { get { return Instance._reservedCount; } }

    public static GameObject Make(string name, GameObject parent)
    {
        return Instance._instantiate(name, parent);
    }

    public static GameObject Get(string name, GameObject parent = null)
    {
        return Instance._get(name, parent);
    }

    public static void Release(GameObject obj)
    {
        Instance._release(obj);
    }

    void Awake()
    {
        _activeCount = 0;
        _reservedCount = 0;
	}

    Pool GetPool(string name)
    {
        if (pool.ContainsKey(name) == false)
        {
            var obj = Resources.Load("Prefabs/" + name);
            if (obj == null)
            {
                obj = Resources.Load("Effects/" + name);
                if (obj == null)
                {
                    obj = Resources.Load("UI_Prefabs/" + name);
                    if (obj == null)
                    {
                        obj = Resources.Load(name);
                        if (obj == null)
                            return null;
                    }
                }
            }

            pool.Add(name, new Pool(obj));
        }
        return pool[name];
    }

    GameObject _instantiate(string name, GameObject parent)
    {
        var p = GetPool(name);
        GameObject ret = Instantiate(p.prefab) as GameObject;
        if (parent != null)
            ret.transform.SetParent(parent.transform, false);
        return ret;
    }

    GameObject _get(string name, GameObject parent = null)
    {
        GameObject ret;
        
        var p = GetPool(name);
        if (p.recycled.Count > 0)
        {
            ret = p.recycled[p.recycled.Count - 1];
            p.recycled.RemoveAt(p.recycled.Count - 1);
            _reservedCount--;
        }
        else
        {
            ret = Instantiate(p.prefab) as GameObject;
            var c = ret.AddComponent<PooledObject>();
            c.objectName = name;
        }

        if (parent == null)
            ret.transform.parent = null;
        else
            ret.transform.parent = parent.transform;

        ret.SetActive(true);
        ret.SendMessage("Initialize", SendMessageOptions.DontRequireReceiver);

        _activeCount++;

        return ret;
    }

    void _release(GameObject obj)
    {
        if (obj == null)
            return;

        var c = obj.GetComponent<PooledObject>();
        if (c == null)
            return;

        if (pool.ContainsKey(c.objectName))
        {
            obj.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
            obj.SetActive(false);
            obj.transform.parent = gameObject.transform;
            pool[c.objectName].recycled.Add(obj);
            _reservedCount++;
            _activeCount--;
        }
    }
}
