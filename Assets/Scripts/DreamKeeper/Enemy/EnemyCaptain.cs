using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 控制动作与行为树
    /// </summary>
    public class EnemyCaptain : IEnemy
    {
        private string aniDead = "Dead";
        private int aniFlail = Animator.StringToHash("Flail");
        private bool isGround;
        private int groundLayerIndex= 1 << LayerMask.NameToLayer("Ground");
        private bool hasFlail;

        public EnemyCaptain(GameObject gameObject) : base(gameObject)
        {
            Type = EnemyType.Warrior;
            MoveSpeed = 1;
            RotSpeed = 1;
            Name = "OspreyCaptain";
            // 更换装备
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["巨斧将军武器"]);
            EnemyMedi.ChangeEquip(GameMainProgram.Instance.dataBaseMgr.dicEnemyEquip["巨斧将军防具"]);
            //boarMono = EnemyMedi.EnemyMono as BoarMono;
            Debug.Log("生成Enemy 巨斧将军的属性：" + CurrentHP + "," + AttackPoint + "," + DefendPoint + "," + CritPoint);
            // 行为树的开启由动画调用
        }

        /// <summary>
        /// 该AI将在创建Enemy 4秒后开启。且需要监听OnWeaponHit
        /// </summary>
        public override void Initialize()
        {
            EnemyMedi.EnemyWeapon.OnWeaponHit.AddListener(WhenWeaponHit);
            CoroutineMgr.Instance.StartCoroutine(EnableAI());
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Update()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 需要使用
            isGround = Physics.Raycast(GameObjectInScene.transform.position+new Vector3(0,0.1f,0), Vector3.down,
                0.2f, groundLayerIndex);
            if(!isGround&&!hasFlail)
            {
                hasFlail = true;
                animator.SetTrigger(aniFlail);
                bt.DisableBehavior();
                CoroutineMgr.Instance.StartCoroutine(FlailDead());
            }
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
            hasFlail = true;
            GameObjectInScene.GetComponent<Collider>().enabled = false;
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.BossDead);
            // 渐隐消失
            CaptainMono captainMono = EnemyMedi.EnemyMono as CaptainMono;
            CoroutineMgr.Instance.StartCoroutine(captainMono.DeadFadeOut());
        }

        public override void WhenPlayerDead()
        {
            bt.SetVariableValue("IsPlayerDead", true);   // 通知行为树
        }

        public void WhenWeaponHit()
        {
            bt.SetVariableValue("HasHit", true);
        }

        private IEnumerator EnableAI()
        {
            yield return new WaitForSeconds(2.6f);
            bt.EnableBehavior();
        }

        private IEnumerator FlailDead()
        {
            yield return new WaitForSeconds(3f);
            Dead();
        }

    }
}