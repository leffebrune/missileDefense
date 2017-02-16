using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyProjectile : MonoBehaviour
{
    GameObject explosion;
    bool exploded = false;
    protected Vector3 targetPos;
    protected Vector3 speed;
    protected float lifeTime;
    protected float createdTime;

    List<GameObject> targets = new List<GameObject>();
    List<GameObject> alreadyHit = new List<GameObject>();

    void Awake()
    {
        explosion = transform.FindChild("explosion").gameObject;
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
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        explosion.SetActive(true);
        speed = Vector3.zero;
        var s = 1.0f;
        var radius = GameBoard.Instance.upgrade.GetExplosionRadius();
        var espeed = GameBoard.Instance.upgrade.GetExplosionSpeed();

        alreadyHit.Clear();

        while (s < radius)
        {
            s += Time.deltaTime * espeed;
            explosion.transform.localScale = new Vector3(s, s, 1);
            EnemyManager.Instance.FindEnemy(transform.position, s * 0.1f, ref targets);
            for (var i = 0; i < targets.Count; i++)
            {
                var go = targets[i];
                if (alreadyHit.Exists(g => g == go))
                    continue;

                alreadyHit.Add(go);
                Game_Playing.Instance.AddScore(10);

                var enemy = go.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.HP -= 5;
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
