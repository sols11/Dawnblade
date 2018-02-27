using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// Enemy编程模板
    /// </summary>
    public class EnemyEyeball : IEnemy
    {
        private string aniDead = "Dead";

        public EnemyEyeball(GameObject gameObject) : base(gameObject)
        {
            Type = EnemyType.Monster;
            MoveSpeed = 1;
            RotSpeed = 1;
            Name = "Eyeball";
            // 更换装备
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["眼怪武器"]);
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["眼怪防具"]);
            //boarMono = EnemyMedi.EnemyMono as BoarMono;
            Debug.Log("生成Enemy 大眼怪的属性：" + CurrentHP + "," + AttackPoint + "," + DefendPoint + "," + CritPoint);
            // 行为树的开启由动画调用
        }


        public override void Release()
        {
            base.Release();
        }

        public override void Update()
        {
            //stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 需要使用
        }

        public override EnemyAction Hurt(EnemyHurtAttr enemyHurtAttr)
        {
            if (!IsDead)
            {
                // 伤害计算
                //int damage = enemyHurtAttr.Attack * enemyHurtAttr.Attack /(enemyHurtAttr.Attack + DefendPoint);
                int damage = enemyHurtAttr.Attack - DefendPoint;
                CurrentHP -= damage;
                Debug.Log("EnemyHurt:" + damage);
            }
            return EnemyAction.Hurt;
        }

        public override void Dead()
        {
            base.Dead();
            animator.SetTrigger(aniDead);
            bt.DisableBehavior();
            GameObjectInScene.GetComponent<Collider>().enabled = false;
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.BossDead);
        }

        public override void WhenPlayerDead()
        {
            bt.SetVariableValue("IsPlayerDead", true);   // 通知行为树
        }

    }
}