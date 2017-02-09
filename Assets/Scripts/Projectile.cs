using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool isEnemy = true;

    Vector3 speed;

    void Start()
    {
        var startPos = new Vector3(Random.Range(-7.0f, 7.0f), 5, 0);
        transform.position = startPos;

        var endPos = new Vector3(Random.Range(-7.0f, 7.0f), -4.0f, 0);

        var dir = (endPos - startPos).normalized;
        speed = dir * 3.0f;
    }

    void Update()
    {
        var p = transform.position;
        p += speed * Time.deltaTime;
        transform.position = p;

        if (p.y < -4.0f)
            Destroy(gameObject);
    }
}
