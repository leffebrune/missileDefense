using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public struct Finger
    {
        public int id;
        public Vector2 start;
        public Vector2 current;
        public bool on;
    }
    public static Touch Instance;

    public delegate void OnStart(int id);
    public delegate void OnEnd(int id);
    public delegate void OnMove(int id, Vector2 pos);

    const int maxTouches = 4;
    Finger[] fingers = new Finger[maxTouches];

    OnStart _onStart = null;
    OnEnd _onEnd = null;
    OnMove _onMove = null;

    bool DebugEnable = false;
    LineRenderer[] debugRenderer;

    void MakeDebugRenderer()
    {
        debugRenderer = new LineRenderer[maxTouches];
        for (var i = 0; i < maxTouches; i++)
        {
            var go = new GameObject("Line");
            go.transform.SetParent(transform);
            var lr = go.AddComponent<LineRenderer>();
            lr.startWidth = 0.2f;
            lr.endWidth = 0.2f;
            lr.numPositions = 2;
            debugRenderer[i] = lr;
            lr.gameObject.SetActive(false);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (var i = 0; i < maxTouches; i++)
        { 
            fingers[i].on = false;
        }
        MakeDebugRenderer();
    }

    public void SetHandler(OnStart _s, OnEnd _e, OnMove _m)
    {
        _onStart = _s;
        _onEnd = _e;
        _onMove = _m;
    }

    void Update()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        if (Input.GetMouseButton(0))
        {
            fingers[0].current = Input.mousePosition;
            if (_onMove != null)
                _onMove(0, fingers[0].current);
        }

        if (Input.GetMouseButtonDown(0))
        {
            fingers[0].on = true;
            fingers[0].start = Input.mousePosition;
            if (_onStart != null)
                _onStart(0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_onEnd != null)
                _onEnd(0);

            fingers[0].on = false;
        }
#else
        var tc = Input.touchCount;

        for (var i = 0; i < tc; i++)
        {
            var t = Input.GetTouch(i);
            var idx = -1;
            var emptySlot = -1;
            for (var j = 0; j < maxTouches; j++)
            {
                if (fingers[j].on && fingers[j].id == t.fingerId)
                {
                    idx = j;
                    break;
                }
                if (!fingers[j].on)
                    emptySlot = j;
            }

            if (idx == -1)
            {
                idx = emptySlot;
                if (idx == -1)
                    continue;
            }

            if (t.phase == TouchPhase.Began)
            {
                fingers[idx].on = true;
                fingers[idx].id = t.fingerId;
                fingers[idx].start = t.position;
                fingers[idx].current = t.position;

                Debug.Log("TOUCH_BEGAN = " + idx + " / " + t.fingerId + " / " + t.position);
                if (_onStart != null)
                    _onStart(fingers[idx].id);
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Debug.Log("TOUCH_MOVE = " + idx + " / " + t.fingerId + " / " + t.position);
                fingers[idx].current = t.position;
                if (_onMove != null)
                    _onMove(fingers[idx].id, fingers[idx].current);
            }
            else if ((t.phase == TouchPhase.Ended) || (t.phase == TouchPhase.Canceled))
            {
                Debug.Log("TOUCH_END = " + idx + " / " + t.fingerId + " / " + t.position);
                fingers[idx].on = false;
                if (_onEnd != null)
                    _onEnd(fingers[idx].id);
            }
        }
#endif
        RenderDebug();
    }

    void RenderDebug()
    {
        if (!DebugEnable)
            return;

        for (var i = 0; i < maxTouches; i++)
        {
            if (!fingers[i].on)
            {
                debugRenderer[i].gameObject.SetActive(false);
                continue;
            }
            
            debugRenderer[i].gameObject.SetActive(true);

            var pos0 = Camera.main.ScreenToWorldPoint(fingers[i].start);
            var pos1 = Camera.main.ScreenToWorldPoint(fingers[i].current);
            pos0.z = 0;
            pos1.z = 0;
            debugRenderer[i].SetPosition(0, pos0);
            debugRenderer[i].SetPosition(1, pos1);
        }
    }

    public bool Get(int id, ref Finger f)
    {
        for (var i = 0; i < maxTouches; i++)
        {
            if (!fingers[i].on)
                continue;

            if (fingers[i].id == id)
            {
                f = fingers[i];
                return true;
            }
        }
        return false;
    }
}
