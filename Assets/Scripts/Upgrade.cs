using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public enum CannonType
    {
        Normal,
        DoubleShot,
        BuckShot,
        TripleShot,
        BirdShot,

        Invalid,
    }

    public int speed = 0;
    public int explosionSpeed = 0;
    public int explosionRadius = 0;

    public int pointsRemain = 0;

    const int maxUpgrade = 10;

    public CannonType cType = CannonType.Normal;

    public void DoUpgrade(int _speed, int _eSpeed, int eRadius)
    {
        if (pointsRemain == 0)
            return;

        speed += _speed;
        pointsRemain -= _speed;
        explosionSpeed += _eSpeed;
        pointsRemain -= _eSpeed;
        explosionRadius += eRadius;
        pointsRemain -= eRadius;

        if (speed > maxUpgrade)
            speed = maxUpgrade;

        if (explosionRadius > maxUpgrade)
            explosionRadius = maxUpgrade;

        if (explosionSpeed > maxUpgrade)
            explosionSpeed = maxUpgrade;
    }

    public int TotalLevel()
    {
        return speed + explosionRadius + explosionSpeed;
    }

    public void Reset()
    {
        speed = explosionSpeed = explosionRadius = 0;
        cType = CannonType.Normal;
    }

    public float GetSpeed()
    {
//         float[] v = { 2.5f, 2.6f, 3.2f, 3.7f, 4,3f, 5.0f, };
//         return v[speed];
        return 2.5f + speed * 0.2f;
    }

    public float GetExplosionSpeed()
    {
        //         float[] v = { 10.0f, 13.0f, 16.0f, 19.0f, 22.0f, 25.0f };
        //         return v[explosionSpeed];
        return 10.0f + explosionSpeed * 2.0f;
    }

    public float GetExplosionRadius()
    {
        //         float[] v = { 8.0f, 9.0f, 10.0f, 11.0f, 12.0f, 14.0f };
        //         return v[explosionRadius];
        return 8.0f + explosionRadius;
    }

    public bool UpgradeAvailable(ref CannonType u1, ref CannonType u2)
    {
        if ((cType == CannonType.Normal) && (TotalLevel() > 5))
        {
            u1 = CannonType.BuckShot;
            u2 = CannonType.DoubleShot;
            return true;
        }
        if ((cType == CannonType.BuckShot) && (TotalLevel() > 10))
        {
            u1 = CannonType.BirdShot;
            u2 = CannonType.Invalid;
            return true;
        }
        if ((cType == CannonType.DoubleShot) && (TotalLevel() > 10))
        {
            u1 = CannonType.TripleShot;
            u2 = CannonType.Invalid;
            return true;
        }
        return false;
    }
}
