using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    struct Finger
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
    Vector2[] touchCurrent = new Vector2[maxTouches];
    Vector2[] touchStart = new Vector2[maxTouches];
    bool[] touchDoing = new bool[maxTouches];

    OnStart _onStart = null;
    OnEnd _onEnd = null;
    OnMove _onMove = null;

    bool DebugEnable = true;
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

    void Start ()
    {
        Instance = this;
        for (var i = 0; i < maxTouches; i++)
            touchDoing[i] = false;
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
            touchDoing[0] = true;
            touchCurrent[0] = Input.mousePosition;
            if (_onMove != null)
                _onMove(0, touchCurrent[0]);
        }
        else
            touchDoing[0] = false;

        if (Input.GetMouseButtonDown(0))
        {
            touchStart[0] = Input.mousePosition;
            if (_onStart != null)
                _onStart(0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_onEnd != null)
                _onEnd(0);
        }
#else
        var tc = Input.touchCount;

        Debug.Log(tc);

        for (var i = 0; i < maxTouches; i++)
        {
            if (tc <= i)
            {        
                touchDoing[i] = false;
                continue;            
            }

            var t = Input.GetTouch(i);
            
            if (t.phase == TouchPhase.Began)
            {
                touchDoing[i] = true;
                touchCurrent[i] = t.position;
                touchStart[i] = t.position;
        
                Debug.Log("TOUCH_BEGAN = " + i + " / " + t.fingerId + " / " + t.position);
                if (_onStart != null)
                    _onStart(i);
            }
            else if (t.phase == TouchPhase.Moved)
            {
                Debug.Log("TOUCH_MOVE = " + i + " / " + t.fingerId + " / " + t.position);
                touchCurrent[i] = t.position;
                if (_onMove != null)
                    _onMove(i, touchCurrent[i]);
            }
            else if ((t.phase == TouchPhase.Ended) || (t.phase == TouchPhase.Canceled))
            {
                Debug.Log("TOUCH_END = " + i + " / " + t.fingerId + " / " + t.position);
                touchDoing[i] = false;
                if (_onEnd != null)
                    _onEnd(i);
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
            if (!touchDoing[i])
            {
                debugRenderer[i].gameObject.SetActive(false);
                continue;
            }
            
            debugRenderer[i].gameObject.SetActive(true);

            var pos0 = Camera.main.ScreenToWorldPoint(touchStart[i]);
            var pos1 = Camera.main.ScreenToWorldPoint(touchCurrent[i]);
            pos0.z = 0;
            pos1.z = 0;
            debugRenderer[i].SetPosition(0, pos0);
            debugRenderer[i].SetPosition(1, pos1);
        }
    }
}
