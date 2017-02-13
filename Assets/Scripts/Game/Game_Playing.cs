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
    public float HP;
    public float score;

    float interval;
    float speed;
    float startTime;

    int remain;

    bool finished;

    public void Start(float startHP, int _remain)
    {
        Instance = this;
        HP = startHP;
        Game_UI.Instance.UpdateHP(HP);
        score = 0;
        Game_UI.Instance.UpdateScore(score);

        interval = 3.0f;
        speed = 1.5f;
        startTime = Time.time;

        remain = _remain;
        finished = false;
        Game_UI.Instance.UpdateRemain(remain);

        GameBoard.Instance.StartCoroutine(Spawn());
    }

    public void OnUpdate()
    {
        interval -= Time.deltaTime * 0.03f;
        if (interval < 1.0f)
            interval = 1.0f;
    }

    public bool CheckFinish(ref Result res)
    {
        if (HP < 0)
        {
            finished = true;
            res.isWin = false;
            return true;
        }
        if ((remain <= 0) && (ProjectileManager.Instance.EnemyCount() == 0))
        {
            finished = true;
            res.isWin = true;
            return true;
        }

        return false;
    }

    public void ReduceHP(float amount)
    {
        HP -= amount;
        Game_UI.Instance.UpdateHP(HP);
    }

    public void AddScore(float amount)
    {
        score += amount;
        Game_UI.Instance.UpdateScore(score);
    }
                
    IEnumerator Spawn()
    {
        while (!finished)
        {
            var startPos = new Vector3(Random.Range(-7.0f, 7.0f), 5, 0);
            var endPos = new Vector3(Random.Range(-7.0f, 7.0f), -4.0f, 0);
            var aSpeed = speed * Random.Range(0.8f, 1.2f);
            ProjectileManager.Instance.MakeEnemy(startPos, endPos, aSpeed);
            remain--;
            Game_UI.Instance.UpdateRemain(remain);

            if (remain <= 0)
                break;

            yield return new WaitForSeconds(interval);
        }
    }
}
