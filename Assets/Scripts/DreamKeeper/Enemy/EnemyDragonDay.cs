using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class EnemyDragonDay : IEnemy
    {
        private string aniDead = "Dead";
        private int aniFly = Animator.StringToHash("Fly");
        private float roarDistance = 15;
        private IPlayer player;
        private Transform target;

        public EnemyDragonDay(GameObject gameObject) : base(gameObject)
        {
            Type = EnemyType.Monster;
            MoveSpeed = 1;
            RotSpeed = 1;
            Name = "Dragon";
            // 更换装备
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["魔龙武器"]);
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["魔龙防具"]);
            //boarMono = EnemyMedi.EnemyMono as BoarMono;
            Debug.Log("生成Enemy 魔怪龙的属性：" + CurrentHP + "," + AttackPoint + "," + DefendPoint + "," + CritPoint);
            // 行为树的开启由动画调用
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Roar()
        {
            // ShakeCamera
            CameraCtrl.Instance.ShakeMainCamera(new Vector3(0, 1, 0), 0, 2); // shake
            // Roared
            target = EnemyMedi.EnemyMono.Target;
            float _dis = Vector3.Distance(target.position, GameObjectInScene.transform.position);
            if (_dis > roarDistance)
                return;
            if (player == null)
                player = target.GetComponent<IPlayerMono>().PlayerMedi.Player;
            if (player != null)
                player.Roared();
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
            //GameObjectInScene.GetComponent<Collider>().enabled = false;
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.BossDead);
        }

        public override void WhenPlayerDead()
        {
            bt.SetVariableValue("IsPlayerDead", true);   // 通知行为树
        }

    }
}