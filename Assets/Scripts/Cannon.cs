using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    int fingerID = -1;
    LineRenderer aimLine;
    Touch.Finger f = new Touch.Finger();

    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        aimLine.numPositions = 2;
        aimLine.SetPosition(0, transform.position);
        aimLine.enabled = false;
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
            var p = go.GetComponent<Projectile>();

            var newdir = q * dir;
            var newEnd = _start + newdir * len;

            var speed = GameBoard.Instance.upgrade.GetSpeed();
            p.Set(_start, newEnd, speed);
            angle += a;
        }
    }

    void DoShoot(Vector3 pos)
    {
        var prefab = Resources.Load<GameObject>("my_projectile");
        var go = Instantiate(prefab);
        var p = go.GetComponent<Projectile>();

        var _start = transform.position;
        _start.z = 0.0f;
        var _end = pos;
        _end.z = 0.0f;

        var speed = GameBoard.Instance.upgrade.GetSpeed();
        p.Set(_start, _end, speed);
    }

    IEnumerator RepeatShoot(Vector3 pos, int count)
    {
        for (var i = 0; i < count; i++)
        {
            DoShoot(pos);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Shoot(int touchID)
    {
        Touch.Finger f = new Touch.Finger();
        if (Touch.Instance.Get(touchID, ref f))
        {
            var cType = GameBoard.Instance.upgrade.cType;
            var pos = Camera.main.ScreenToWorldPoint(f.current);

            if (cType == Upgrade.CannonType.Normal)
                DoShoot(pos);
            else if (cType == Upgrade.CannonType.BuckShot)
                Buckshot(pos, 3);
            else if (cType == Upgrade.CannonType.BirdShot)
                Buckshot(pos, 5);
            else if (cType == Upgrade.CannonType.DoubleShot)
                StartCoroutine(RepeatShoot(pos, 2));
            else if (cType == Upgrade.CannonType.TripleShot)
                StartCoroutine(RepeatShoot(pos, 3));
        }
    }
}
