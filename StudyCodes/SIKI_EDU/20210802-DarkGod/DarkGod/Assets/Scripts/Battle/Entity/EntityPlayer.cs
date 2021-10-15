using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }

    public override Vector2 CalcTargetDir()
    {
        EntityMonster monster = FindClosedTarget();
        if (monster != null)
        {
            Vector3 target = monster.GetPos();
            Vector3 self = GetPos();
            Vector2 dir = new Vector2(target.x - self.x, target.z - self.z);
            return dir.normalized;
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 寻找最近怪物
    /// </summary>
    private EntityMonster FindClosedTarget()
    {
        List<EntityMonster> mList = battleMgr.GetEntityMonsters();
        if (mList == null || mList.Count == 0)
        {
            return null;
        }

        Vector3 self = GetPos();
        float dis = 0;
        EntityMonster closedMonster = null;
        
        foreach (var m in mList)
        {
            var disTmp = Vector3.Distance(self,m.GetPos());
            if (closedMonster == null || disTmp < dis)
            {
                closedMonster = m;
                dis = disTmp;
            }
        }

        return closedMonster;
    }
}