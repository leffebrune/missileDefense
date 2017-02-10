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
        while (s < 10.0f)
        {
            s += Time.deltaTime * 10.0f;
            explosion.transform.localScale = new Vector3(s, s, 1);
            var go = ProjectileManager.Instance.FindEnemy(transform.position, s * 0.1f);
            if (go != null)
            {
                ProjectileManager.Instance.Remove(go);
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
