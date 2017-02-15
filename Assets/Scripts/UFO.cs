using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    bool goLeft = true;
    float speed = 1.0f;
    float interval = 3.0f;
    
    void Start()
    {

    }

    public void Set(Vector3 _startPos, float _speed, float _interval)
    {
        transform.position = _startPos;
        speed = _speed;
        interval = _interval;
        StartCoroutine(SpawnProjectiles());
    }

    void Update()
    {
        var p = transform.position;

        if (goLeft)
        {
            p.x -= speed * Time.deltaTime;
            if (p.x < -6.0f)
                goLeft = false;
        }
        else
        {
            p.x += speed * Time.deltaTime;
            if (p.x > 6.0f)
                goLeft = true;
        }

        transform.position = p;
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            var endPos = new Vector3(Random.Range(-7.0f, 7.0f), -4.0f, 0);
            var aSpeed = speed * Random.Range(0.8f, 1.2f);
            EnemyManager.Instance.MakeMissile(EnemyManager.EnemyType.Missile, transform.position, endPos);
        }
    }
}
