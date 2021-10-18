using System;
using UnityEngine;

public class MonsterController: Controller
{
   private void Update()
   {
      // AI逻辑
      if (isMove)
      {
         SetDir();
         SetMove();
      }
   }
   
   private void SetDir()
   {
      float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
      Vector3 eulerAngles = new Vector3(0, angle, 0);
      transform.localEulerAngles = eulerAngles;
   }
   
   private void SetMove()
   {
      ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
      // 为了修正资源，在不勾选 apply root 的前提下让怪物落地进行的特殊对应
      ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
   }
   
   
}