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
        while (true)
        {
            var startPos = new Vector3(Random.Range(-7.0f, 7.0f), 5, 0);
            var endPos = new Vector3(Random.Range(-7.0f, 7.0f), -4.0f, 0);
            ProjectileManager.Instance.MakeEnemy(startPos, endPos, 3.0f);
            yield return new WaitForSeconds(3.0f);
        }
    }
}
