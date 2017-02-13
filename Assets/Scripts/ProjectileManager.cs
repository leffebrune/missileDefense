using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    List<GameObject> listMine = new List<GameObject>();
    List<GameObject> listEnemy = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }
    
    public void MakeMine(Vector3 _startPos, Vector3 _targetPos, float _speed)
    {
        var prefab = Resources.Load<GameObject>("my_projectile");
        var go = Instantiate(prefab);
        var p = go.GetComponent<Projectile>();

        _startPos.z = 0.0f;
        _targetPos.z = 0.0f;

        p.Set(_startPos, _targetPos, _speed);

        listMine.Add(go);
    }

    public void MakeEnemy(Vector3 _startPos, Vector3 _targetPos, float _speed)
    {
        var prefab = Resources.Load<GameObject>("projectile");
        var go = Instantiate(prefab);
        var p = go.GetComponent<Projectile>();

        _startPos.z = 0.0f;
        _targetPos.z = 0.0f;

        p.Set(_startPos, _targetPos, _speed);

        listEnemy.Add(go);
    }

    public void Remove(GameObject p)
    {
        listMine.Remove(p);
        listEnemy.Remove(p);

        Destroy(p);
    }

    public void Clear()
    {
        foreach (var go in listMine)
            GameObject.Destroy(go);
        foreach (var go in listEnemy)
            GameObject.Destroy(go);

        listMine.Clear();
        listEnemy.Clear();
    }

    public GameObject FindEnemy(Vector3 pos, float radius)
    {
        pos.z = 0;
        foreach (var go in listEnemy)
        {
            var ePos = go.transform.position;
            ePos.z = 0;

            if ((ePos - pos).magnitude < radius)
                return go;
        }
        return null;
    }
}
