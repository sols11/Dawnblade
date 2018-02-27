using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SFramework
{
    /// <summary>
    ///+ 敌人武器所需属性
    ///+ EnemyMediator
    ///武器的控制层
    /// </summary>
    public abstract class IEnemyWeapon : IWeaponMono
	{
        public EnemyMediator EnemyMedi { get; set; }
        public bool DefeatedDown = false;
        public bool IsOnlyPlayer = true;
        //public bool HasHit { get; set; }     // 是否击中Player，这种bool比较适合复杂的判断情况
        public UnityEvent OnWeaponHit;       // 击中Player后触发，然后可以通过这个回调方法让行为树得知是否击中Player
        protected PlayerHurtAttr PlayerHurtAttr { get; set; }
        protected IPlayerMono OnlyPlayerMono { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            //一定要初始化
            TransformForward = Vector3.zero;
            PlayerHurtAttr = new PlayerHurtAttr();
        }

    }
}