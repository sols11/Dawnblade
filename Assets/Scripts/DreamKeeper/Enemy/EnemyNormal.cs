using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// Enemy编程模板
    /// </summary>
    public class EnemyNormal : IEnemy
    {
        private string aniHurt = "Hurt";

        public EnemyNormal(GameObject gameObject) : base(gameObject)
        {
            Type = EnemyType.Monster;
            MoveSpeed = 0;
            RotSpeed = 0;
            Name = "Scarecrow";
            // 更换装备
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["野猪武器"]);
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["野猪防具"]);
            Debug.Log("生成Enemy 稻草人的属性：" + CurrentHP + "," + AttackPoint + "," + DefendPoint + "," + CritPoint);
            // 行为树的开启由动画调用
        }


        public override void Release()
        {
            base.Release();
        }

        public override void Update()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 需要使用
        }

        public override EnemyAction Hurt(EnemyHurtAttr enemyHurtAttr)
        {
            if (!IsDead)
            {
                animator.SetTrigger(aniHurt);
                return EnemyAction.Hurt;
            }
            return EnemyAction.Miss;
        }

        public override void Dead()
        {
            base.Dead();
        }

    }
}