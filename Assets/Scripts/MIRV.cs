using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIRV : EnemyMissile
{   
    protected override bool ForceExplode()
    {
        if (transform.position.y < 2.5f)
            return true;
        return false;
    }

    protected override void Explode()
    {
        for (var i = 0; i < 5; i++)
        {
            var newTarget = targetPos;
            newTarget.x += Random.Range(-2.0f, 2.0f);
            newTarget.x = Mathf.Clamp(newTarget.x, -GameUtil.BoardWidth, GameUtil.BoardWidth);
            EnemyManager.Instance.MakeMissile(GameData.EnemyType.MIRV_RV, transform.position, newTarget);
        }
        HP = 0;
    }
}
