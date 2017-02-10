using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isEnemy = true;
    bool exploded = false;
    protected Vector3 targetPos;
    protected Vector3 speed;
    protected float lifeTime;
    protected float createdTime;
    
    void Start()
    {

    }

    public void Set(Vector3 _startPos, Vector3 _targetPos, float _speed)
    {
        transform.position = _startPos;
        targetPos = _targetPos;

        var d = _targetPos - _startPos;
        speed = _speed * d.normalized;
        lifeTime = d.magnitude / _speed;
        createdTime = Time.time;
    }

    void Update()
    {
        if (exploded)
            return;
        var p = transform.position;
        p += speed * Time.deltaTime;
        transform.position = p;

        if (Time.time - createdTime > lifeTime)
        {
            exploded = true;
            OnExplode();
        }
    }

    protected virtual void OnExplode()
    {
        ProjectileManager.Instance.Remove(gameObject);
    }
}
