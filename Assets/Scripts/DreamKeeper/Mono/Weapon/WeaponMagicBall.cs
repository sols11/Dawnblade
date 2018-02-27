using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 武器类，负责伤害计算，碰撞检测和传参,特效生成
    /// MagicBall一开始即存在
    /// </summary>
    public class WeaponMagicBall : IPlayerWeapon
    {
        public Rigidbody Rgbd { get; private set; }
        private KatanaAnimEvent katanaAnimEvent; // 对应的PlayerMono
        private string defendEffectPath;
        private string parriedEffectPath;
        private Collider trigger;

        public override void Initialize()
        {
            base.Initialize();
            hitEffectPath = @"Particles\Boom_Splash";   // Player的攻击频率过快，且敌人可能有多个，所以缓冲池会产生多个
            defendEffectPath = @"Particles\Boom_White";
            parriedEffectPath = @"Particles\Boom_Orange";
            katanaAnimEvent = PlayerMedi.PlayerMono as KatanaAnimEvent;
            trigger = GetComponent<Collider>();
            Rgbd = GetComponent<Rigidbody>();
        }

        public void Cast()
        {
            transform.localPosition = Vector3.up;
            gameObject.SetActive(true);
            trigger.enabled = true;
            TransformForward = PlayerMedi.PlayerMono.transform.forward;
            Rgbd.velocity = TransformForward * 5;
        }

        private void InstantiateEffect()
        {
            if (EnemyReturn == EnemyAction.Defend)
            {
                if (!katanaAnimEvent.IsHeavyAttack()) // 防住
                    resourcesMgr.LoadAsset(defendEffectPath, true, transform.position, Quaternion.identity);
            }
            else if (EnemyReturn == EnemyAction.Parry)
            {
                if (katanaAnimEvent.IsHeavyAttack()) //被挡开
                {
                    resourcesMgr.LoadAsset(parriedEffectPath, true, transform.position, Quaternion.identity);
                    katanaAnimEvent.Parried();
                }
                else // 防住
                    resourcesMgr.LoadAsset(defendEffectPath, true, transform.position, Quaternion.identity);
            }
            else
                resourcesMgr.LoadAsset(hitEffectPath, true, transform.position, Quaternion.Euler(90, 0, 0));
        }

        protected override void OnTriggerEnter(Collider col)
        {
            // 自行开关Trigger
            if (Rgbd.velocity.magnitude < 1)
            {
                Debug.Log("Trigger off");
                trigger.enabled = false;
                return;
            }
            if (col.gameObject.layer == (int)ObjectLayer.Enemy)
            {
                if (!enemyList.Contains(col.gameObject))//攻击存在性判断
                {
                    enemyList.Add(col.gameObject);
                    RealAttack = BasicAttack * AttackFactor;
                    //传参给enemy告知受伤
                    EnemyHurtAttribute.ModifyAttr((int)RealAttack, VelocityForward, VelocityVertical, TransformForward);
                    EnemyReturn = col.GetComponent<IEnemyMono>().Hurt(EnemyHurtAttribute);
                    //特效
                    InstantiateEffect();
                }
            }

        }  // end_function
    }
}