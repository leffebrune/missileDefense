using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    bool aimStart = false;
    LineRenderer aimLine;
    Touch.Finger f = new Touch.Finger();

    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        aimLine.numPositions = 2;
        aimLine.SetPosition(0, transform.position);
        aimLine.enabled = false;
        Touch.Instance.SetHandler(
            (id) =>
            {                
                if (Touch.Instance.Get(id, ref f))
                {
                    var _start = Camera.main.ScreenToWorldPoint(f.start);
                    var diff = _start - gameObject.transform.position;
                    diff.z = 0.0f;
                    if (diff.magnitude < 0.5f)
                    {
                        aimLine.enabled = true;
                        aimLine.SetPosition(1, transform.position);
                        aimStart = true;
                    }
                }
            },
            (id) =>
            {
                if (aimStart)
                {
                    aimLine.enabled = false;
                    aimStart = false;
                    Shoot();
                }
            },
            (id, pos) =>
            {
                if (aimStart)
                {
                    if (Touch.Instance.Get(id, ref f))
                    {
                        var _curr = Camera.main.ScreenToWorldPoint(f.current);
                        _curr.z = 0.0f;
                        aimLine.SetPosition(1, _curr);
                    }
                }
            }
            );
    }

    void Shoot()
    {
        Touch.Finger f = new Touch.Finger();
        if (Touch.Instance.Get(0, ref f))
        {
            var prefab = Resources.Load<GameObject>("my_projectile");
            var go = Instantiate(prefab);
            var p = go.GetComponent<Projectile>();

            var _start = transform.position;
            _start.z = 0.0f;
            var _end = Camera.main.ScreenToWorldPoint(f.current);
            _end.z = 0.0f;

            p.Set(_start, _end, 3.0f);
        }
    }
}
