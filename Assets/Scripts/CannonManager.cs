using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public List<Cannon> Cannons;

    void Start()
    {
        Touch.Instance.SetHandler(
            (id) =>
            {
                foreach (var c in Cannons)
                    c.OnTouchStart(id);
            },
            (id) =>
            {
                foreach (var c in Cannons)
                    c.OnTouchEnd(id);
            },
            (id, pos) =>
            {
                foreach (var c in Cannons)
                    c.OnTouchMove(id, pos);
            }
            );
    }
}
