using UnityEngine;
using System;

public class PooledObject : MonoBehaviour
{
    public string objectName;

    public Vector3 initialScale;

    TrailRenderer[] trailRenderer;

    void Awake()
    {
        initialScale = transform.localScale;

        trailRenderer = GetComponentsInChildren<TrailRenderer>();
    }

    void Initialize()
    {
        if (trailRenderer != null)
            Array.ForEach(trailRenderer, r => r.Clear());
    }
}
