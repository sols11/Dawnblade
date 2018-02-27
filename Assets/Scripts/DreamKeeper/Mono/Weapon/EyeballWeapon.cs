using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// Trigger的开关交由Animation处理
    /// </summary>
    public class EyeballWeapon : IEnemyWeapon
    {
        // 击中时产生的特效，可以在制作Prefab时写好，如果不用的话留空即可
        public string hitEffectPath = @"Particles\EnemyEffect\Recoil_Metal";

        protected override void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.layer == (int)ObjectLayer.Player)
            {
                RealAttack = BasicAttack * AttackFactor;
                //传参给Player告知受伤
                TransformForward = EnemyMedi.EnemyMono.transform.forward;
                PlayerHurtAttr.ModifyAttr((int)RealAttack, VelocityForward, VelocityVertical, TransformForward, canDefeatedFly,DefeatedDown);
                if (IsOnlyPlayer)
                {
                    if (OnlyPlayerMono == null)
                        OnlyPlayerMono = col.GetComponent<IPlayerMono>();
                    // 击倒在地会由Hurt判断
                    OnlyPlayerMono.Hurt(PlayerHurtAttr);
                }
                if (hitEffectPath != string.Empty)
                    resourcesMgr.LoadAsset(hitEffectPath, true, transform.position, Quaternion.identity);

                //HasHit = true;
                //OnWeaponHit.Invoke();
            }
        }  // end_function
    }
}