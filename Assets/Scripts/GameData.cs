using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public enum EnemyType
    {
        Missile,
        HeavyMissile,
        MIRV,
        MIRV_RV,
        UFO,
        Boss,
        BossMissile,

        Invalid
    }

    public struct EnemyInfo
    {
        public string prefab;

        public float minSpeed;
        public float maxSpeed;

        public int HP;
    }

    public enum CannonType
    {
        Normal,
        Fast,
        Heavy,
        DoubleShot,

        SuperFast,
        Cluster,

        HeavyFast,
        Nuke,

        TripleShot,
        DoubleCluster,

        Invalid,
    }

    public class CannonInfo
    {
        public struct Level
        {
            public float speed;
            public float explosionRadius;
            public float explosionSpeed;
            public int damage;
            public int cost;
        }

        public struct Upgrade
        {
            public int level;
            public CannonType next;
        }
        
        public int maxLevel;

        public Level[] info;
        public List<Upgrade> upgrades = new List<Upgrade>();
    }

    List<GameObject> listEnemy = new List<GameObject>();

    public Dictionary<EnemyType, EnemyInfo> enemyInfo = new Dictionary<EnemyType, EnemyInfo>();
    public Dictionary<CannonType, CannonInfo> cannonInfo = new Dictionary<CannonType, CannonInfo>();

    void AddEnemy(EnemyType _type, string _prefab, float _minSpeed, float _maxSpeed, int _hp)
    {
        enemyInfo[_type] = new EnemyInfo() { prefab = _prefab, minSpeed = _minSpeed, maxSpeed = _maxSpeed, HP = _hp };
    }

    void MakeLevel(ref CannonInfo.Level i, int cost, int damage, float speed, float eRad, float eSpeed)
    {
        i.cost = cost;
        i.damage = damage;
        i.speed = speed;
        i.explosionRadius = eRad;
        i.explosionSpeed = eSpeed;
    }

    void MakeUpgrade(ref CannonInfo ci, int lv, CannonType next)
    {
        ci.upgrades.Add(new CannonInfo.Upgrade() { level = lv, next = next });
    }

    void MakeCannonInfo()
    {
        var ci = new CannonInfo();
        ci.maxLevel = 5;
        ci.info = new CannonInfo.Level[5];
        MakeLevel(ref ci.info[0], 0, 3, 2.5f, 8.0f, 10.0f);
        MakeLevel(ref ci.info[1], 1, 3, 2.6f, 9.0f, 11.0f);
        MakeLevel(ref ci.info[2], 2, 4, 2.7f, 9.0f, 12.0f);
        MakeLevel(ref ci.info[3], 3, 4, 2.8f, 10.0f, 13.0f);
        MakeLevel(ref ci.info[4], 4, 5, 2.9f, 10.0f, 13.0f);
        
        MakeUpgrade(ref ci, 5, CannonType.Fast);
        MakeUpgrade(ref ci, 5, CannonType.Heavy);
        MakeUpgrade(ref ci, 5, CannonType.DoubleShot);

        cannonInfo[CannonType.Normal] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 5;
        ci.info = new CannonInfo.Level[5];
        MakeLevel(ref ci.info[0], 0, 2, 4.5f, 7.0f, 12.0f);
        MakeLevel(ref ci.info[1], 2, 2, 4.7f, 7.0f, 12.0f);
        MakeLevel(ref ci.info[2], 3, 2, 4.9f, 7.0f, 12.0f);
        MakeLevel(ref ci.info[3], 4, 2, 5.1f, 9.0f, 14.0f);
        MakeLevel(ref ci.info[4], 5, 2, 5.3f, 9.0f, 14.0f);

        MakeUpgrade(ref ci, 5, CannonType.SuperFast);
        MakeUpgrade(ref ci, 5, CannonType.Cluster);

        cannonInfo[CannonType.Fast] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 5;
        ci.info = new CannonInfo.Level[5];
        MakeLevel(ref ci.info[0], 0, 10, 1.5f, 15.0f, 18.0f);
        MakeLevel(ref ci.info[1], 2, 12, 1.5f, 16.0f, 18.0f);
        MakeLevel(ref ci.info[2], 3, 14, 1.5f, 17.0f, 18.0f);
        MakeLevel(ref ci.info[3], 4, 16, 1.5f, 18.0f, 18.0f);
        MakeLevel(ref ci.info[4], 5, 18, 1.5f, 19.0f, 18.0f);

        MakeUpgrade(ref ci, 5, CannonType.HeavyFast);
        MakeUpgrade(ref ci, 5, CannonType.Nuke);

        cannonInfo[CannonType.Heavy] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 5;
        ci.info = new CannonInfo.Level[5];
        MakeLevel(ref ci.info[0], 0, 4, 2.5f, 10.0f, 10.0f);
        MakeLevel(ref ci.info[1], 2, 4, 2.7f, 10.0f, 10.0f);
        MakeLevel(ref ci.info[2], 3, 4, 2.9f, 10.0f, 10.0f);
        MakeLevel(ref ci.info[3], 4, 5, 2.9f, 10.0f, 10.0f);
        MakeLevel(ref ci.info[4], 5, 6, 2.9f, 10.0f, 10.0f);

        MakeUpgrade(ref ci, 5, CannonType.TripleShot);
        MakeUpgrade(ref ci, 5, CannonType.DoubleCluster);

        cannonInfo[CannonType.DoubleShot] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 4, 8.0f, 10.0f, 10.0f);

        cannonInfo[CannonType.SuperFast] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 2, 4.0f, 10.0f, 10.0f);

        cannonInfo[CannonType.Cluster] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 15, 3.5f, 15.0f, 20.0f);

        cannonInfo[CannonType.HeavyFast] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 20, 1.0f, 20.0f, 10.0f);

        cannonInfo[CannonType.Nuke] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 6, 3.5f, 10.0f, 10.0f);

        cannonInfo[CannonType.TripleShot] = ci;

        ci = new CannonInfo();
        ci.maxLevel = 1;
        ci.info = new CannonInfo.Level[1];
        MakeLevel(ref ci.info[0], 0, 3, 4.0f, 10.0f, 10.0f);

        cannonInfo[CannonType.DoubleCluster] = ci;
    }


    void Awake()
    {
        AddEnemy(EnemyType.Missile, "projectile", 0.5f, 1.2f, 3);
        AddEnemy(EnemyType.HeavyMissile, "heavy_missile", 0.2f, 0.4f, 10);
        AddEnemy(EnemyType.MIRV, "MIRV", 0.5f, 0.6f, 10);
        AddEnemy(EnemyType.MIRV_RV, "projectile", 0.6f, 1.1f, 1);
        AddEnemy(EnemyType.UFO, "UFO", 0.4f, 0.6f, 8);
        AddEnemy(EnemyType.Boss, "Boss", 0.0f, 0.0f, 100);
        AddEnemy(EnemyType.BossMissile, "projectile", 0.8f, 0.8f, 3);
        MakeCannonInfo();
    }

    public EnemyInfo GetEnemyInfo(EnemyType _type)
    {
        return enemyInfo[_type];
    }
}
