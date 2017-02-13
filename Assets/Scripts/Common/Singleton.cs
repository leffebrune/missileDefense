using UnityEngine;
using System.Collections.Generic;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Initialize();
            return _instance;
        }
    }

    static void Initialize()
    {
        if (_instance == null)
        {
            var go = GameObject.Find(typeof(T).Name);
            if (go == null)
                go = new GameObject(typeof(T).Name);

            _instance = go.GetComponent<T>();
            if (_instance == null)
                _instance = go.AddComponent<T>();
        }
    }    
}
