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
    public float score;

    float interval;
    float speed;
    float startTime;

    int remain;
    int remainUFO;

    bool finished;

    public void Start(int _remain)
    {
        Instance = this;
        Game_UI.Instance.UpdateHP(GameBoard.Instance.HP);
        score = 0;
        Game_UI.Instance.UpdateScore(score);
        
        var d = GameBoard.Instance.day;
        var mCount = 5 + d;
        var hmCount = (2 + d) / 3;
        var uCount = d / 4;
        if (uCount < 0)
            uCount = 0;
        
        interval = 5.0f - d * 0.1f;
        if (interval < 3.5f)
            interval = 3.5f;

        startTime = Time.time;

        finished = false;

        EnemySpawner.Instance.Set(GameData.EnemyType.Missile, mCount, interval);
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
        if (GameBoard.Instance.HP <= 0)
        {
            finished = true;
            res.isWin = false;
            return true;
        }
        if ((EnemySpawner.Instance.RemainCount() <= 0) && (EnemyManager.Instance.EnemyCount() == 0))
        {
            finished = true;
            res.isWin = true;
            return true;
        }

        return false;
    }

    public void ReduceHP(float amount)
    {
        GameBoard.Instance.HP -= amount;
        Game_UI.Instance.UpdateHP(GameBoard.Instance.HP);
    }

    public void AddScore(float amount)
    {
        score += amount;
        Game_UI.Instance.UpdateScore(score);
    }

    IEnumerator OpeningSeq()
    {
        Game_UI.Instance.textDay.gameObject.SetActive(true);
        Game_UI.Instance.textDay.text = "Day " + GameBoard.Instance.day.ToString();
        yield return new WaitForSeconds(2.0f);
        Game_UI.Instance.textDay.gameObject.SetActive(false);
        EnemySpawner.Instance.StartSpawn();
    }
}
