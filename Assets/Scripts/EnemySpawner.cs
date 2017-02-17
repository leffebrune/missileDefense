using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    int[] remains = new int[(int)GameData.EnemyType.Invalid];
    float[] intervals = new float[(int)GameData.EnemyType.Invalid];

    void Awake()
    {
    }

    public void Clear()
    {
        for (var i = 0; i < remains.Length; i++)
        {
            remains[i] = 0;
            intervals[i] = 0;
        }
        StopAllCoroutines();
    }

    public void Set(GameData.EnemyType t, int count, float interval)
    {
        var idx = (int)t;
        remains[idx] = count;
        intervals[idx] = interval;
    }

    public void StartSpawn()
    {
        for (var i = 0; i < remains.Length; i++)
        {
            StartCoroutine(Spawn(i));
        }
    }

    IEnumerator Spawn(int idx)
    {
        var _type = (GameData.EnemyType)idx;
        while (remains[idx] > 0)
        {
            if (_type == GameData.EnemyType.UFO)
                EnemyManager.Instance.MakeUFO(_type);
            else
                EnemyManager.Instance.MakeMissile(_type, GameUtil.GetMissileStart(), GameUtil.GetMissileTarget());
            remains[idx]--;
            Game_UI.Instance.UpdateRemain();
            yield return new WaitForSeconds(intervals[idx]);
        }
    }

    public string GetText()
    {
        var ret = "Remain : " + Environment.NewLine;

        for (var i = 0; i < remains.Length; i++)
        {
            if (remains[i] == 0)
                continue;
            var _type = (GameData.EnemyType)i;
            ret += _type.ToString() + " : " + remains[i] + Environment.NewLine;
        }
        return ret;
    }

    public int RemainCount()
    {
        var ret = 0;
        for (var i = 0; i < remains.Length; i++)
            ret += remains[i];
        return ret;
    }
}
