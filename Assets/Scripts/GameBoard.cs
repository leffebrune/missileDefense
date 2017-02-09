using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        var prefab = Resources.Load<GameObject>("projectile");
        while (true)
        {
            var go = Instantiate(prefab);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
