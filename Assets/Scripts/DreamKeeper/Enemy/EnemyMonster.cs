using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
	public class EnemyMonster : IEnemy
	{
		//private string aniHurt="Hurt";
        private string aniDead = "Dead";
        private MonsterMono monsterMono;

		public EnemyMonster(GameObject gameObject):base(gameObject)
		{
            Type = EnemyType.Monster;
            MoveSpeed = 1;
            RotSpeed = 1;
            Name = "Monster";
            // 更换装备
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["泰坦之拳"]);
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["泰坦之盾"]);
            monsterMono = EnemyMedi.EnemyMono as MonsterMono;
            monsterMono.SkillWeaponInitialize();
            Debug.Log("生成Enemy 泰坦的属性：" + CurrentHP + "," + AttackPoint + "," + DefendPoint + "," + CritPoint);
            // 行为树的开启由动画调用，用的是BehaviorTree组件自带的方法
		}

		public override void Update() {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 需要使用
        }

        public override void FixedUpdate() { }

		public override EnemyAction Hurt(EnemyHurtAttr enemyHurtAttr)
		{
            if(!IsDead)
			{
                // Idle下且非Transition才触发动画，其他扣血但无动画，如果Transition时那么Trigger会延迟触发
                //if (animator.IsInTransition(0)&&stateInfo.IsName("Idle"))
                animator.SetTrigger("Hurt");
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
            bt.SetVariableValue("IsPlayerDead",true);   // 通知行为树
        }

    }
}