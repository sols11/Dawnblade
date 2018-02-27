using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SFramework
{
    /// <summary>
    /// 输入管理者
    /// 由于动作游戏有许多动画切换，我们目前仅对PlayerPeace移动使用
    /// 【禁止移动的原理】
    /// 首先CanMoveInput在创建角色时赋值为true，并注册MoveInput事件，接受输入
    /// 当对话开始时，CanMove=false使角色无法在空间中移动，且Speed=0，使其退出Walk状态；CanMoveInput=false不接受输入
    /// 对话完毕，CanMoveInput=true，角色销毁，CanMoveInput=false
    /// 最关键的地方就是要用InputMgr来拒绝接受输入，否则输入很可能还会保留，而且记得要让角色回到Idle状态
    /// </summary>
    public class InputMgr : IGameMgr
    {
        public bool CanMoveInput { get; set; }
        public UnityEvent OnDirectionAxis = new UnityEvent();
        //public UnityEvent OnButtonDrinkDown = new UnityEvent();
        //public UnityEvent OnButtonAvoidDown = new UnityEvent();


        public InputMgr(GameMainProgram gameMain) : base(gameMain)
        {
        }

        public override void Update()
        {
            if (CanMoveInput)
                OnDirectionAxis.Invoke();
            //if (Input.GetButtonDown("Drink"))
            //{
            //    OnButtonDrinkDown.Invoke();
            //}
            //if (Input.GetButtonDown("Avoid"))
            //{
            //    OnButtonAvoidDown.Invoke();
            //}
        }


    }
}
