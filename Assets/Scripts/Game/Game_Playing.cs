using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Playing
{
    public static Game_Playing Instance;
    public float HP;
    public float score;

    float interval;
    float speed;
    float startTime;

    public void Start(float startHP)
    {
        Instance = this;
        HP = startHP;
        Game_UI.Instance.UpdateHP(HP);
        score = 0;
        Game_UI.Instance.UpdateScore(score);

        interval = 3.0f;
        speed = 2.0f;
        startTime = Time.time;

        GameBoard.Instance.StartCoroutine(Spawn());
    }

    public void OnUpdate()
    {
        interval -= Time.deltaTime * 0.03f;
        if (interval < 1.0f)
            interval = 1.0f;

        speed += Time.deltaTime * 0.015f;
        if (speed > 4.0f)
            speed = 4.0f;
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
        while (HP > 0)
        {
            var startPos = new Vector3(Random.Range(-7.0f, 7.0f), 5, 0);
            var endPos = new Vector3(Random.Range(-7.0f, 7.0f), -4.0f, 0);
            var aSpeed = speed + Random.Range(-0.5f, 0.5f);
            ProjectileManager.Instance.MakeEnemy(startPos, endPos, aSpeed);
            yield return new WaitForSeconds(interval);
        }
    }
}
