using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public enum EnemyType
    {
        Missile,
        HeavyMissile,
        UFO,

        Invalid
    }

    public struct Info
    {
        public string prefab;
        public float baseSpeed;
        public float speedIncrease;
        public float maxSpeed;
    }
    
    List<GameObject> listEnemy = new List<GameObject>();

    Dictionary<EnemyType, Info> infos = new Dictionary<EnemyType, Info>();

    void Awake()
    {
        infos[EnemyType.Missile] = new Info() { prefab = "projectile", baseSpeed = 0.8f, maxSpeed = 1.7f, speedIncrease = 0.1f };
        infos[EnemyType.HeavyMissile] = new Info() { prefab = "heavy_missile", baseSpeed = 0.15f, maxSpeed = 0.5f, speedIncrease = 0.03f };
        infos[EnemyType.UFO] = new Info() { prefab = "UFO", baseSpeed = 1.0f, maxSpeed = 1.0f, speedIncrease = 0.0f };
    }

    public void MakeMissile(EnemyType _type, Vector3 _startPos, Vector3 _targetPos)
    {
        var _info = new Info();
        if (!infos.TryGetValue(_type, out _info))
            return;

        var prefab = Resources.Load<GameObject>(_info.prefab);
        var go = Instantiate(prefab);
        var p = go.GetComponent<EnemyMissile>();

        p.Set(_startPos, _targetPos, GameUtil.GetMissileSpeed(_info));

        listEnemy.Add(go);
    }

    public void MakeUFO(EnemyType _type)
    {
        var _info = new Info();
        if (!infos.TryGetValue(_type, out _info))
            return;
        
        var startPos = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(3.5f, 4.5f), 0);
        var prefab = Resources.Load<GameObject>(_info.prefab);
        var go = Instantiate(prefab);
        var p = go.GetComponent<UFO>();

        p.Set(startPos, 1.0f, 3.0f);

        listEnemy.Add(go);
    }

    public void Remove(GameObject p)
    {
        listEnemy.Remove(p);
        Destroy(p);
    }

    public void Clear()
    {
        foreach (var go in listEnemy)
            GameObject.Destroy(go);

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

    public int EnemyCount()
    {
        return listEnemy.Count;
    }
}
