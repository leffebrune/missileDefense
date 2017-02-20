using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{    
    List<Enemy> listEnemy = new List<Enemy>();

    void Awake()
    {
    }

    void Update()
    {
        listEnemy.RemoveAll(e =>
        {
            if (e.HP <= 0)
            {
                Destroy(e.gameObject);
                return true;
            }
            else
                return false;
        });
    }

    public void MakeMissile(GameData.EnemyType _type, Vector3 _startPos, Vector3 _targetPos)
    {
        var _info = GameData.Instance.GetEnemyInfo(_type);

        var prefab = Resources.Load<GameObject>(_info.prefab);
        var go = Instantiate(prefab);
        var p = go.GetComponent<EnemyMissile>();

        p.Set(_startPos, _targetPos, Random.Range(_info.minSpeed, _info.maxSpeed));
        p.HP = _info.HP;

        listEnemy.Add(go.GetComponent<Enemy>());
    }

    public void MakeUFO(GameData.EnemyType _type)
    {
        var _info = GameData.Instance.GetEnemyInfo(_type);

        var startPos = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(3.5f, 4.5f), 0);
        var prefab = Resources.Load<GameObject>(_info.prefab);
        var go = Instantiate(prefab);
        var p = go.GetComponent<UFO>();

        p.Set(startPos, Random.Range(_info.minSpeed, _info.maxSpeed), 4.0f);
        p.HP = _info.HP;

        listEnemy.Add(go.GetComponent<Enemy>());
    }

    public void MakeBoss(GameData.EnemyType _type)
    {
        var _info = GameData.Instance.GetEnemyInfo(_type);

        var startPos = new Vector3(0.0f, 4.0f, 0);
        var prefab = Resources.Load<GameObject>(_info.prefab);
        var go = Instantiate(prefab);
        var p = go.GetComponent<Boss>();

        p.Set(startPos);
        p.HP = _info.HP;

        listEnemy.Add(go.GetComponent<Enemy>());
    }

    public void Clear()
    {
        foreach (var go in listEnemy)
            GameObject.Destroy(go.gameObject);

        listEnemy.Clear();
    }

    public void FindEnemy(Vector3 pos, float radius, ref List<GameObject> result)
    {
        result.Clear();
        pos.z = 0;
        foreach (var go in listEnemy)
        {
            var ePos = go.transform.position;
            ePos.z = 0;

            if ((ePos - pos).magnitude < radius)
                result.Add(go.gameObject);
        }
    }

    public int EnemyCount()
    {
        return listEnemy.Count;
    }
}
