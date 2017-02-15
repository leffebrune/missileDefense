using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyProjectile : Projectile
{
    GameObject explosion;
    
    void Awake()
    {
        explosion = transform.FindChild("explosion").gameObject;
    }

    protected override void OnExplode()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        explosion.SetActive(true);
        speed = Vector3.zero;
        var s = 1.0f;
        var radius = GameBoard.Instance.upgrade.GetExplosionRadius();
        var espeed = GameBoard.Instance.upgrade.GetExplosionSpeed();
        while (s < radius)
        {
            s += Time.deltaTime * espeed;
            explosion.transform.localScale = new Vector3(s, s, 1);
            var go = EnemyManager.Instance.FindEnemy(transform.position, s * 0.1f);
            if (go != null)
            {
                Game_Playing.Instance.AddScore(10);
                EnemyManager.Instance.Remove(go);
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
