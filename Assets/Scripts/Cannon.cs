using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    int fingerID = -1;
    LineRenderer aimLine;
    SpriteRenderer sr;
    Touch.Finger f = new Touch.Finger();

    float cooldownTime = -1.0f;
    Color enableCol = Color.white;
    Color disableCol = Color.red;

    public int Index = 0;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        aimLine = GetComponent<LineRenderer>();
        aimLine.numPositions = 2;
        aimLine.SetPosition(0, transform.position);
        aimLine.enabled = false;
    }

    void Update()
    {
        if (cooldownTime > 0)
        {
            sr.color = disableCol;
            cooldownTime -= Time.deltaTime;
        }
        else
        {
            sr.color = enableCol;
        }        
    }

    public void OnTouchStart(int id)
    {
        if (fingerID != -1)
            return;

        if (Touch.Instance.Get(id, ref f))
        {
            var _start = Camera.main.ScreenToWorldPoint(f.start);
            var diff = _start - gameObject.transform.position;
            diff.z = 0.0f;
            if (diff.magnitude < 0.5f)
            {
                aimLine.enabled = true;
                aimLine.SetPosition(1, transform.position);
                fingerID = id;
            }
        }
    }

    public void OnTouchMove(int id, Vector2 pos)
    {
        if (fingerID == id)
        {
            if (Touch.Instance.Get(id, ref f))
            {
                var _curr = Camera.main.ScreenToWorldPoint(f.current);
                _curr.z = 0.0f;
                aimLine.SetPosition(1, _curr);
            }
        }
    }

    public void OnTouchEnd(int id)
    {
        if (fingerID == id)
        {
            aimLine.enabled = false;
            fingerID = -1;
            Shoot(id);
        }
    }

    void Buckshot(Vector3 pos, int count)
    {
        var prefab = Resources.Load<GameObject>("my_projectile");

        var _start = transform.position;
        _start.z = 0.0f;
        var _end = pos;
        _end.z = 0.0f;

        var dir = (_end - _start).normalized;
        var len = (_end - _start).magnitude;

        float a = 10.0f;
        float angle = ((count / 2) + 1) - count;
        angle *= a;
        for (var i = 0; i < count; i++)
        {
            Quaternion q = Quaternion.Euler(0, 0, angle);
            var go = Instantiate(prefab);
            var p = go.GetComponent<MyProjectile>();

            var newdir = q * dir;
            var newEnd = _start + newdir * len;

            var cType = Session.Instance.cInfo[Index];
            var info = GameData.Instance.cannonInfo[cType._type].info[cType.level - 1];
            
            p.Set(_start, newEnd, info.speed, info);
            angle += a;
        }
    }

    void DoShoot(Vector3 pos)
    {
        var prefab = Resources.Load<GameObject>("my_projectile");
        var go = Instantiate(prefab);
        var p = go.GetComponent<MyProjectile>();

        var _start = transform.position;
        _start.z = 0.0f;
        var _end = pos;
        _end.z = 0.0f;

        var cType = Session.Instance.cInfo[Index];
        var info = GameData.Instance.cannonInfo[cType._type].info[cType.level - 1];
        
        p.Set(_start, _end, info.speed, info);
    }

    IEnumerator RepeatShoot(Vector3 pos, int count)
    {
        for (var i = 0; i < count; i++)
        {
            DoShoot(pos);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator RepeatBuckShoot(Vector3 pos, int count, int bullet)
    {
        for (var i = 0; i < count; i++)
        {
            Buckshot(pos, bullet);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Shoot(int touchID)
    {
        if (cooldownTime > 0.0f)
            return;

        cooldownTime = 1.0f;

        Touch.Finger f = new Touch.Finger();
        if (Touch.Instance.Get(touchID, ref f))
        {
            var cType = Session.Instance.cInfo[Index]._type;
            var pos = Camera.main.ScreenToWorldPoint(f.current);

            switch (cType)
            {
                case GameData.CannonType.Normal:
                case GameData.CannonType.Fast:
                case GameData.CannonType.Heavy:
                case GameData.CannonType.SuperFast:
                case GameData.CannonType.HeavyFast:
                case GameData.CannonType.Nuke:
                    DoShoot(pos);
                    break;
                case GameData.CannonType.DoubleShot:
                    StartCoroutine(RepeatShoot(pos, 2));
                    break;
                case GameData.CannonType.TripleShot:
                    StartCoroutine(RepeatShoot(pos, 3));
                    break;
                case GameData.CannonType.Cluster:
                    Buckshot(pos, 5);
                    break;
                case GameData.CannonType.DoubleCluster:
                    StartCoroutine(RepeatBuckShoot(pos, 2, 3));
                    break;
            }
        }
    }
}
