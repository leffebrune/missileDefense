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
        speed = 1.5f + d * 0.15f;
        if (speed > 5.0f)
            speed = 5.0f;

        interval = 3.0f - d * 0.1f;
        if (interval < 1.5f)
            interval = 1.5f;

        startTime = Time.time;

        remain = 9 + d;
        remainUFO = -10 + d;
        if (remainUFO <= 0)
            remainUFO = 0;
        finished = false;
        Game_UI.Instance.UpdateRemain(remain);

        GameBoard.Instance.StartCoroutine(Spawn());
        GameBoard.Instance.StartCoroutine(SpawnUFO());
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
        GameBoard.Instance.HP -= amount;
        Game_UI.Instance.UpdateHP(GameBoard.Instance.HP);
    }

    public void AddScore(float amount)
    {
        score += amount;
        Game_UI.Instance.UpdateScore(score);
    }

    IEnumerator Spawn()
    {
        Game_UI.Instance.textDay.gameObject.SetActive(true);
        Game_UI.Instance.textDay.text = "Day " + GameBoard.Instance.day.ToString();
        yield return new WaitForSeconds(2.0f);
        Game_UI.Instance.textDay.gameObject.SetActive(false);
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

            yield return new WaitForSeconds(interval * Random.Range(0.8f, 1.2f));
        }
    }

    IEnumerator SpawnUFO()
    {
        if (remainUFO > 0)
        {
            while (!finished)
            {
                var startPos = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(3.5f, 4.5f), 0);
                ProjectileManager.Instance.MakeUFO(startPos, 2.0f, interval);
                remainUFO--;

                if (remainUFO <= 0)
                    break;

                yield return new WaitForSeconds(6.0f);
            }
        }
    }
}
