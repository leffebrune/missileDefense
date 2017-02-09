using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    bool aimStart = false;
    void Start()
    {
        Touch.Instance.SetHandler(
            (id) =>
            {
                aimStart = true;
            },
            (id) =>
            {
                Shoot();
            },
            (id, pos) =>
            {

            }
            );
    }

    void Shoot()
    {
        var prefab = Resources.Load<GameObject>("projectile");
        var go = Instantiate(prefab);        
    }
}
