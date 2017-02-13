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

    void Shoot(int touchID)
    {
        Touch.Finger f = new Touch.Finger();
        if (Touch.Instance.Get(touchID, ref f))
        {
            var prefab = Resources.Load<GameObject>("my_projectile");
            var go = Instantiate(prefab);
            var p = go.GetComponent<Projectile>();

            var _start = transform.position;
            _start.z = 0.0f;
            var _end = Camera.main.ScreenToWorldPoint(f.current);
            _end.z = 0.0f;

            var speed = GameBoard.Instance.upgrade.GetSpeed();
            p.Set(_start, _end, speed);
        }
    }
}
