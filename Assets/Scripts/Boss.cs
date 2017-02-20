using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    void Start()
    {

    }

    public void Set(Vector3 _startPos)
    {
        transform.position = _startPos;
        StartCoroutine(ExecPatterns());
    }
    
    IEnumerator ExecPatterns()
    {
        var p1 = new Vector3(transform.position.x, GameUtil.GroundLevel, 0);
        while (true)
        {
            yield return new WaitForSeconds(4.0f);
            for (var i = 0; i < 4; i++)
            {
                var p2 = p1;
                var p3 = p1;
                const float w = 1.5f;
                p2.x = p1.x + ((i + 1) * w);
                p3.x = p1.x - ((i + 1) * w);
                EnemyManager.Instance.MakeMissile(GameData.EnemyType.BossMissile, transform.position, p1);
                EnemyManager.Instance.MakeMissile(GameData.EnemyType.BossMissile, transform.position, p2);
                EnemyManager.Instance.MakeMissile(GameData.EnemyType.BossMissile, transform.position, p3);
                yield return new WaitForSeconds(1.5f);
            }

            yield return new WaitForSeconds(3.0f);
            for (var i = 0; i < 8; i++)
            {
                var endPos = new Vector3(Random.Range(-7.0f, 7.0f), GameUtil.GroundLevel, 0);
                EnemyManager.Instance.MakeMissile(GameData.EnemyType.Missile, transform.position, endPos);
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(5.0f);
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 5; j++)
                    EnemyManager.Instance.MakeMissile(GameData.EnemyType.MIRV, GameUtil.GetMissileStart(), GameUtil.GetMissileTarget());
                yield return new WaitForSeconds(6.0f);
            }
        }
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {

        }
    }
}
