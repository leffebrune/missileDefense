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
    }

    int speed = 0;
    int explosionSpeed = 0;
    int explosionRadius = 0;

    CannonType cType = CannonType.Normal;

    public void DoUpgrade(int _speed, int _eSpeed, int eRadius)
    {
        speed += _speed;
        explosionSpeed += _eSpeed;
        explosionRadius += eRadius;

        if (speed > 5)
            speed = 5;

        if (explosionRadius > 5)
            explosionRadius = 5;

        if (explosionSpeed > 5)
            explosionSpeed = 5;
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
        float[] v = { 2.5f, 2.8f, 3.2f, 3.7f, 4,3f, 5.0f };
        return v[speed];
    }

    public float GetExplosionSpeed()
    {
        float[] v = { 10.0f, 13.0f, 16.0f, 19.0f, 22.0f, 25.0f };
        return v[explosionSpeed];
    }

    public float GetExplosionRadius()
    {
        float[] v = { 8.0f, 12.0f, 16.0f, 20.0f, 24.0f, 29.0f };
        return v[explosionRadius];
    }
}
