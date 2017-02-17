using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : Singleton<Session>
{
    public struct CannonInfo
    {
        public int level;
        public GameData.CannonType _type;
    }

    public int points;
    public int HP;
    public int MaxHP;
    public int day;

    public CannonInfo[] cInfo = new CannonInfo[3];

    public void Reset()
    {
        points = 0;
        MaxHP = HP = 100;
        day = 0;

        cInfo[0] = new CannonInfo() { level = 1, _type = GameData.CannonType.Normal };
        cInfo[1] = new CannonInfo() { level = 1, _type = GameData.CannonType.Normal };
        cInfo[2] = new CannonInfo() { level = 1, _type = GameData.CannonType.Normal };
    }

    public int GetRepairCost()
    {
        return (MaxHP - HP) / 30;
    }

    public void Repair()
    {
        var c = GetRepairCost();
        if (c <= points)
        {
            points -= c;
            HP = MaxHP;
        }
    }
      
    public bool CanLevelUp(int cIdx)
    {
        var curr = cInfo[cIdx];
        if (GameData.Instance.cannonInfo[curr._type].maxLevel <= curr.level)
            return false;

        var c = GameData.Instance.cannonInfo[curr._type].info[curr.level];

        return (c.cost <= points);
    }

    public void LevelUp(int cIdx)
    {
        var curr = cInfo[cIdx];
        if (GameData.Instance.cannonInfo[curr._type].maxLevel <= curr.level)
            return;

        var c = GameData.Instance.cannonInfo[curr._type].info[curr.level];

        if (c.cost <= points)
        {
            curr.level++;
            points -= c.cost;
            cInfo[cIdx] = curr;
        }
    }

    public bool CanUpgrade(int cIdx, int uIdx)
    {
        var curr = cInfo[cIdx];
        if (GameData.Instance.cannonInfo[curr._type].upgrades[uIdx].level <= curr.level)
            return true;
        else
            return false;
    }

    public void DoUpgrade(int cIdx, int uIdx)
    {
        var curr = cInfo[cIdx];
        var u = GameData.Instance.cannonInfo[curr._type].upgrades[uIdx];

        if (u.level <= curr.level)
        {
            curr.level = 1;
            curr._type = u.next;
            cInfo[cIdx] = curr;
        }
    }
}
