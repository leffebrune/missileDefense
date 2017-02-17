using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : Enemy
{
    protected Vector3 targetPos;
    protected Vector3 startPos;

    protected Vector3 velocity;

    protected float createdTime;
    protected float lifeTime;

    protected float speed;

    public int damage = 10;

    void Start()
    {

    }

    public void Set(Vector3 _startPos, Vector3 _targetPos, float _speed)
    {
        transform.position = _startPos;
        targetPos = _targetPos;

        var d = _targetPos - _startPos;

        speed = _speed;

        velocity = _speed * d.normalized;
        lifeTime = d.magnitude / _speed;
        createdTime = Time.time;

        activated = true;
    }

    void Update()
    {
        if (!activated)
            return;
        var p = transform.position;
        p += velocity * Time.deltaTime;
        transform.position = p;

        if ((Time.time - createdTime > lifeTime) || ForceExplode())
        {
            activated = false;
            Explode();
        }
    }

    protected virtual bool ForceExplode()
    {
        return false;
    }

    protected virtual void Explode()
    {
        Game_Playing.Instance.ReduceHP(damage);
        HP = 0;
    }
}
