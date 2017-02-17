using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Playing
{
    public struct Result
    {
        public bool isWin;
    };

    public static Game_Playing Instance;

    float interval;
    float startTime;

    public void Start(int _remain)
    {
        Instance = this;
        Game_UI.Instance.UpdateHP();
        
        var d = Session.Instance.day;
        var mCount = 5 + d;
        var mirvCount = (d - 2) / 3;
        var hmCount = (2 + d) / 2;
        var uCount = d / 4;
        if (uCount < 0)
            uCount = 0;
        if (mirvCount < 0)
            mirvCount = 0;

//         mCount = 0;
//         mirvCount = 0;
//         hmCount = 0;
//         uCount = 1;

        interval = 5.0f - d * 0.1f;
        if (interval < 3.5f)
            interval = 3.5f;

        startTime = Time.time;
        

        EnemySpawner.Instance.Set(GameData.EnemyType.Missile, mCount, interval);
        EnemySpawner.Instance.Set(GameData.EnemyType.MIRV, mirvCount, interval * 4.5f);
        EnemySpawner.Instance.Set(GameData.EnemyType.HeavyMissile, hmCount, interval * 4);
        EnemySpawner.Instance.Set(GameData.EnemyType.UFO, uCount, interval * 5);

        Game_UI.Instance.UpdateRemain();

        GameBoard.Instance.StartCoroutine(OpeningSeq());
    }

    public void OnUpdate()
    {
        interval -= Time.deltaTime * 0.03f;
        if (interval < 1.0f)
            interval = 1.0f;
    }

    public bool CheckFinish(ref Result res)
    {
        if (Session.Instance.HP <= 0)
        {
            res.isWin = false;
            return true;
        }
        if ((EnemySpawner.Instance.RemainCount() <= 0) && (EnemyManager.Instance.EnemyCount() == 0))
        {
            res.isWin = true;
            return true;
        }

        return false;
    }

    public void ReduceHP(float amount)
    {
        Session.Instance.HP -= (int)amount;
        Game_UI.Instance.UpdateHP();
    }
    
    IEnumerator OpeningSeq()
    {
        Game_UI.Instance.textDay.gameObject.SetActive(true);
        Game_UI.Instance.textDay.text = "Day " + Session.Instance.day.ToString();
        yield return new WaitForSeconds(2.0f);
        Game_UI.Instance.textDay.gameObject.SetActive(false);
        EnemySpawner.Instance.StartSpawn();
    }
}
