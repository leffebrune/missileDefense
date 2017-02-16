using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public enum EnemyType
    {
        Missile,
        HeavyMissile,
        UFO,

        Invalid
    }

    public struct EnemyInfo
    {
        public string prefab;

        public float minSpeed;
        public float maxSpeed;

        public int HP;
    }
    
    List<GameObject> listEnemy = new List<GameObject>();

    Dictionary<EnemyType, EnemyInfo> enemyInfo = new Dictionary<EnemyType, EnemyInfo>();

    void AddEnemy(EnemyType _type, string _prefab, float _minSpeed, float _maxSpeed, int _hp)
    {
        enemyInfo[_type] = new EnemyInfo() { prefab = _prefab, minSpeed = _minSpeed, maxSpeed = _maxSpeed, HP = _hp };
    }

    void Awake()
    {
        AddEnemy(EnemyType.Missile, "projectile", 0.6f, 1.4f, 3);
        AddEnemy(EnemyType.HeavyMissile, "heavy_missile", 0.2f, 0.4f, 10);
        AddEnemy(EnemyType.UFO, "projectile", 0.5f, 0.8f, 8);
    }

    public EnemyInfo GetEnemyInfo(EnemyType _type)
    {
        return enemyInfo[_type];
    }
}
