using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil
{
    public static float GroundLevel = -4.2f;
    public static float BoardWidth = 7.0f;

    public static Vector3 GetMissileStart()
    {
        return new Vector3(Random.Range(-BoardWidth, BoardWidth), 5, 0);
    }

    public static Vector3 GetMissileTarget()
    {
        return new Vector3(Random.Range(-BoardWidth, BoardWidth), GroundLevel, 0);
    }

    public static float GetMissileSpeed(EnemyManager.Info i)
    {
        var d = GameBoard.Instance.day;
        var s = Mathf.Max(i.baseSpeed + d * i.speedIncrease, i.maxSpeed);
        return s;
    }
}
