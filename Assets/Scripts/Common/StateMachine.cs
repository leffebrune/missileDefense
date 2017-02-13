using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    class State
    {
        public T id;
        public Action<T, object> onEnter;
        public Action onUpdate;
        public Action<T> onExit;
        public Action<string, object> onEvent;
    }

    Dictionary<T, State> sm = new Dictionary<T, State>();
    State current = null;
    float lastEnterTime = 0.0f;
    Action<string, object> globalEventHandler = null;

    float reservedTime = -1.0f;
    T reservedState;

    public void AddState(T _id, Action<T, object> _onEnter, Action _onUpdate = null, Action<T> _onExit = null, Action<string, object> _onEvent = null)
    {
        sm.Add(_id, new State() { id = _id, onEnter = _onEnter, onUpdate = _onUpdate, onExit = _onExit, onEvent = _onEvent });
    }

    public void SetEventHandler(Action<string, object> _handler)
    {
        globalEventHandler = _handler;
    }

    public void Enter(T id, object param = null)
    {
        if (!sm.ContainsKey(id))
            return;
        
        reservedTime = -1;
        var s = sm[id];
        T prev = id;

        if (current != null)
        {
            prev = current.id;
            if (current.onExit != null)
                current.onExit(id);
        }

        lastEnterTime = Time.time;
        current = s;
        if (current.onEnter != null)
            current.onEnter(prev, param);
    }

    public void OnUpdate()
    {
        if (reservedTime > 0.0f)
        {
            reservedTime -= Time.deltaTime;
            if (reservedTime < 0.0f)
                Enter(reservedState);
        }

        if ((current != null) && (current.onUpdate != null))
            current.onUpdate();
    }

    public void SendEvent(string id, object param = null)
    {
        if (globalEventHandler != null)
            globalEventHandler(id, param);
        if ((current != null) && (current.onEvent != null))
            current.onEvent(id, param);
    }

    public T GetCurrent()
    {
        return current.id;
    }

    public void Reserve(T id, float time)
    {
        reservedState = id;
        reservedTime = time;
    }

    public float StateElapsed()
    {
        return Time.time - lastEnterTime;
    }
}
