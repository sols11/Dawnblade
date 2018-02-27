using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class EyeballMono : IEnemyMono
    {
        public FarawayWeapon skillWeapon;  // Enemy的第二武器
        private EnemyEyeball eyeball;

        public override void Initialize()
        {
            base.Initialize();
            eyeball = EnemyMedi.Enemy as EnemyEyeball;
            skillWeapon.Initialize();
            EnemyMedi.UpdateEnemyWeapon(skillWeapon);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.collider.gameObject.layer == (int)ObjectLayer.Wall)
            {
                iEnemyWeapon.WeaponCollider.isTrigger = false;
                CameraCtrl.Instance.ShakeMainCamera(new Vector3(0, 0.5f, 0), 0, 3);
            }
        }

        /// <summary>
        /// 给行为树反射调用
        /// </summary>
        public void ShakeCamera()
        {
            CameraCtrl.Instance.ShakeMainCamera(new Vector3(0, 1, 0), 0, 2); // shake
        }
        /// <summary>
        /// 给行为树反射调用
        /// </summary>
        public void UseWeapon()
        {
            skillWeapon.UseWeapon();
        }
    }
}