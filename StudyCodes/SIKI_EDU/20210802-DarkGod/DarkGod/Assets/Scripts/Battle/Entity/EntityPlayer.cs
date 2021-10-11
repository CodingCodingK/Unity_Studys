using UnityEngine;

public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }
}